using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Models
{
    public class Instance : DbObject
    {
        private SqlConnectionStringBuilder builder;

        public event EventHandler<QueryEventArgs> QueryAdded;
        public event EventHandler<QueryEventArgs> QueryRemoved;

        public string ConnectionString { get => builder.ConnectionString; }

        public List<Folder> Databases { get => getFolder("Databases").Objects; }

        private List<Query> Queries { get; set; }



        public Instance(string name, string server, bool sqlAuth = false, string username = "", string password = "", bool encrypt = false) : base(null, name)
        {
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = server.Trim();
            builder.Encrypt = encrypt;
            builder.IntegratedSecurity = !sqlAuth;
            if (!sqlAuth)
            {
                builder.Remove("User ID");
                builder.Remove("Password");
            }
            else
            {
                builder.UserID = username.Trim();
                builder.Password = password.Trim();
            }
            Queries = new List<Query>();
            Objects.Add(new Folder("Databases", this));
        }

        public Instance(Connection connection) : this(connection.Name, connection.Server, !connection.SqlAuth, connection.Username, connection.Password, connection.Encrypted)
        {
        }

        public override void LoadData(Folder folder)
        {
            if (folder.Name == "Databases")
                GetDatabases();
        }

        private void GetDatabases()
        {
            Databases.Clear();
            SqlConnection conn = new SqlConnection(ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT name FROM master.sys.databases", conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var db = new Database(this, reader.GetString(0));
                Databases.Add(db);
            }
            conn.Close();
        }


        protected virtual void OnQueryAdded(Query query)
        {
            QueryAdded?.Invoke(this, new QueryEventArgs(query));
        }

        protected virtual void OnQueryRemoved(Query query)
        {
            QueryRemoved?.Invoke(this, new QueryEventArgs(query));
        }

        public void AddQuery(Query query)
        {
            Queries.Add(query);
            OnQueryAdded(query);
        }

        public void RemoveQuery(Query query)
        {
            Queries.Remove(query);
            OnQueryRemoved(query);
        }

        public bool CheckConnection()
        {
            try
            {
                SqlConnection conn = new SqlConnection(ConnectionString);
                conn.Open();
                conn.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void CreateDatabase(string name)
        {
            var q = new Query(this, $"CREATE DATABASE {name}", $"CREATE {name}");
            AddQuery(q);
            q.Execute();
        }


        /// <summary>
        /// Getes property's value from SERVERPROPERTY function
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public string GetPropertyValue(string property)
        {
            SqlConnection conn = new SqlConnection(ConnectionString);
            conn.Open();
            SqlCommand command = new SqlCommand($"SELECT SERVERPROPERTY('{property}')", conn);
            var res = command.ExecuteScalar().ToString();
            conn.Close();
            return res;
        }
    }

    public class QueryEventArgs : EventArgs
    {
        public Query Query { get; set; }
        public QueryEventArgs(Query query)
        {
            Query = query;
        }
    }
}
