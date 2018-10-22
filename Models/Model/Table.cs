using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace SSMSLite
{
    public class Table : DbObject
    {
        public List<Folder> Columns { get => getFolder("Columns").Objects; }

        public Table(DbObject parent, string name) : base(parent, name)
        {
            Objects.Add(new Folder("Columns", this));
        }

        public override void LoadData(Folder Folder)
        {
            if(Folder.Name == "Columns")
                GetColumns();
        }

        public void GetColumns()
        {
            Columns.Clear();
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            SqlCommand cmd = new SqlCommand(@"SELECT distinct c.COLUMN_NAME, c.IS_NULLABLE, c.DATA_TYPE, ISNULL(c.CHARACTER_MAXIMUM_LENGTH, COL_LENGTH(c.TABLE_NAME, c.COLUMN_NAME)), 
                                                case when t.COLUMN_NAME = c.COLUMN_NAME AND t.CONSTRAINT_NAME like 'PK_%' then 'YES' else 'NO' end as PRIMARY_KEY
                                                FROM INFORMATION_SCHEMA.COLUMNS c
                                                left join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE t on t.TABLE_NAME = c.TABLE_NAME AND c.COLUMN_NAME = t.COLUMN_NAME
                                                WHERE c.TABLE_NAME = '" + Name + "'", conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var c = new Column(this, reader.GetString(0))
                {
                    Nullable = reader.GetString(1) == "YES",
                    DataType = reader.GetString(2),
                    MaxLength = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    PrimaryKey = reader.GetString(4) == "YES"
                };
                Columns.Add(c);
            }
            conn.Close();
        }

        public void Select1000()
        {
            Query q = new Query(Database, $"SELECT TOP 1000 *\nFROM {Name}\n", $"SELECT {Name}");
            Database.Instance.AddQuery(q);
            q.Execute();
        }

        public void NewQuery()
        {
            Database.Instance.AddQuery(new Query(Database));
        }

        public void Create(string name)
        {
            Database.NewTable(name);
        }

        public void Design()
        {
            Query q = new Query(Database, $"ALTER TABLE {Name}", $"ALTER {Name}");
            Database.Instance.AddQuery(q);
        }

        public void Insert()
        {
            Query q = new Query(Database, $"INSERT INTO {Name} VALUES ()", $"INSERT {Name}");
            Database.Instance.AddQuery(q);
        }

        public void Update()
        {
            Query q = new Query(Database, $"UPDATE {Name}\nSET XXX\nWHERE XXX", $"UPDATE {Name}");
            Database.Instance.AddQuery(q);
        }

        public void DeleteQuery()
        {
            Query q = new Query(Database, $"DELETE FROM {Name}\nWHERE XXX", $"DELETE {Name}");
            Database.Instance.AddQuery(q);
        }

        public void Rename(string name)
        {
            Query q = new Query(Database, $"EXEC sp_rename '{Database.Name}.{Name}', '{name}'");
            q.Execute();
        }

        public void Delete()
        {
            Query q = new Query(Database, $"DROP TABLE {Name}");
            q.Execute();
        }

        public void AddColumn()
        {
            var q = new Query(Database, $"ALTER TABLE {Name} ADD \n" +
                $"column_name VARCHAR(20) NULL, \n" +
                $"column_name VARCHAR(20) NULL", "ADD column");
            Database.Instance.AddQuery(q);
        }
    }
}
