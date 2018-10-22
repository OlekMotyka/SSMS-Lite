using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SSMSLite
{
    public class Database : DbObject
    {
        public Instance Instance { get; set; }

        public string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(Instance.ConnectionString);
                builder.InitialCatalog = Name;
                return builder.ConnectionString;
            }
        }

        public List<Folder> Tables { get => getFolder("Tables").Objects; }
        public List<Folder> Views { get => getFolder("Views").Objects; }
        public List<Folder> Programmability { get => getFolder("Programmability").Objects; }
        public List<Folder> Procedures { get => getFolder("Programmability").getFolder("Procedures").Objects; }
        public List<Folder> Functions { get => getFolder("Programmability").getFolder("Functions").Objects; }

        public Database(Instance instance, string name) : base(null, name)
        {
            Instance = instance;
            Database = this;
            Objects.Add(new Folder("Tables", this));
            Objects.Add(new Folder("Views", this));
            Objects.Add(new Folder("Programmability", this));
            Programmability.Add(new Folder("Procedures", this));
            Programmability.Add(new Folder("Functions", this));
        }

        public override void LoadData(Folder folder)
        {
            if (folder.Name == "Tables")
                LoadTables();
            if (folder.Name == "Views")
                LoadViews();
            if (folder.Name == "Procedures")
                LoadProcedures();
            if (folder.Name == "Functions")
                LoadFunctions();
        }

        public void LoadTables()
        {
            Tables.Clear();
            DataTable reader = DbAccess.ExecuteReader(ConnectionString, 
                $"SELECT TABLE_SCHEMA, TABLE_NAME FROM {Name}.INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME");
            foreach(DataRow row in reader.Rows)
            {
                var t = new Table(this, (string)row["TABLE_NAME"]);
                t.Schema = (string)row["TABLE_SCHEMA"];
                Tables.Add(t);
            }
        }

        public void LoadViews()
        {
            Views.Clear();

            DataTable reader = DbAccess.ExecuteReader(ConnectionString, 
                $"SELECT TABLE_SCHEMA, TABLE_NAME FROM {Name}.INFORMATION_SCHEMA.TABLES where TABLE_TYPE = 'VIEW'");
            foreach (DataRow row in reader.Rows)
            {
                var v = new View(this, (string)row["TABLE_NAME"]);
                v.Schema = (string)row["TABLE_SCHEMA"];
                Views.Add(v);
            }
        }

        public void LoadProcedures()
        {
            Views.Clear();
            DataTable reader = DbAccess.ExecuteReader(ConnectionString, 
                $"SELECT [ROUTINE_SCHEMA], [ROUTINE_NAME] FROM [{Name}].[INFORMATION_SCHEMA].[ROUTINES] where ROUTINE_TYPE = 'PROCEDURE'");
            foreach (DataRow row in reader.Rows)
            {
                var v = new Procedure(this, (string)row["ROUTINE_NAME"]);
                v.Schema = (string)row["ROUTINE_SCHEMA"];
                Procedures.Add(v);
            }
        }

        public void LoadFunctions()
        {
            Views.Clear();
            DataTable reader = DbAccess.ExecuteReader(ConnectionString, 
                $"SELECT [ROUTINE_SCHEMA], [ROUTINE_NAME],[DATA_TYPE] FROM [{Name}].[INFORMATION_SCHEMA].[ROUTINES] where ROUTINE_TYPE = 'FUNCTION'");
            foreach (DataRow row in reader.Rows)
            {
                var v = new Function(this, (string)row["ROUTINE_NAME"]);
                v.Schema = (string)row["ROUTINE_SCHEMA"];
                v.Returns = (string)row["DATA_TYPE"];
                Functions.Add(v);
            }
        }

        public void NewQuery()
        {
            Instance.AddQuery(new Query(this));
        }

        public void Create(string name)
        {
            var q = new Query(Instance, $"CREATE DATABASE {name}");
            q.Execute();
        }

        public void NewTable(string name)
        {
            var q = new Query(this, QS.CreateTable(name), $"CREATE {name}");
            Instance.AddQuery(q);
        }

        public void NewView(string name)
        {
            var q = new Query(this, QS.CreateView(name), "CREATE VIEW");
            Instance.AddQuery(q);
        }

        public void Backup(string path)
        {
            var q = new Query(Instance, QS.Backup(Name, path));
            q.Execute();
        }

        public void Restore(string path)
        {
            var q = new Query(Instance, QS.Restore(Name, path));
            q.Execute();
        }

        public void Rename(string name)
        {
            var q = new Query(Instance, QS.RenameDatabase(Name, name), $"RENAME {Name}");
            q.Execute();
        }

        public void Drop()
        {
            var q = new Query(Instance, $"DROP DATABASE {Name}", $"DROP {Name}");
            q.Execute();
        }

        public void NewProcedure()
        {
            var q = new Query(this, QS.NewProcedure(), "CREATE PROCEDURE");
            Instance.AddQuery(q);
        }

        public void NewTableFunction()
        {
            var q = new Query(this, QS.NewTableFunction(), "CREATE FUNCTION");
            Instance.AddQuery(q);
        }

        public void NewScalarFunction()
        {
            var q = new Query(this, QS.NewScalarFunction(), "CREATE FUNCTION");
            Instance.AddQuery(q);
        }
    }
}
