using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Models
{
    public class Query
    {
        public event EventHandler<QueryEventArgs> BeforeExecute;
        public event EventHandler<QueryEventArgs> AfterExecute;

        public string Name { get; set; }
        public string QueryText { get; set; }
        public string ConsoleResult { get; set; }
        public DataTable Result { get; set; }
        public Database Database { get; set; }

        public Query(DbObject database = null, string query = "", string name = "New Query")
        {
            if (database is Instance)
                Database = (database as Instance).Databases.Where(x => x.Name == "master").First() as Database;
            else
                Database = database as Database;

            QueryText = query;
            Name = name;
        }

        public void Execute()
        {
            BeforeExecute?.Invoke(this, new QueryEventArgs(this));
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            SqlCommand cmd = new SqlCommand(QueryText, conn);
            try
            {
                conn.Open();
                if (QueryText.Trim().Substring(0, 10).ToLower().StartsWith("select"))
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    Result = new DataTable();
                    Result.Load(reader);
                }
                else
                {
                    Result = null;
                    int affected = cmd.ExecuteNonQuery();
                    ConsoleResult = "Affected " + affected + " rows";
                }
            }
            catch (Exception ex)
            {
                Result = null;
                ConsoleResult = ex.Message;
            }
            finally
            {
                conn.Close();
            }
            AfterExecute?.Invoke(this, new QueryEventArgs(this));
        }
    }
}
