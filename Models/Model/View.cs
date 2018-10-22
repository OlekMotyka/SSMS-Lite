using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSMSLite
{
    public class View : DbObject
    {
        public List<Folder> Columns { get => getFolder("Columns").Objects; }

        public View(DbObject parent, string name) : base(parent, name)
        {
            Objects.Add(new Folder("Columns", this));
        }

        public override void LoadData(Folder folder)
        {
            if (folder.Name == "Columns")
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
            Query q = new Query(Database, $"SELECT TOP (1000) *\nFROM {Name}", $"SELECT/{Name}");
            Database.Instance.AddQuery(q);
            q.Execute();
        }

        public void Create(string name)
        {
            Database.NewView(name);
        }

        public void Design()
        {
            var q = (string)DbAccess.ExecuteScalar(Database.ConnectionString, $"select definition from sys.objects o join sys.sql_modules m on m.object_id = o.object_id where o.object_id = object_id( '{Name}') and o.type = 'V'");
            q = q.Replace("CREATE", "ALTER");
            Query query = new Query(Database, q, $"ALTER {Name}");
            Database.Instance.AddQuery(query);
        }

        public void Rename(string name)
        {
            Query q = new Query(Database, $"EXEC sp_rename '{Database.Name}.{Name}', '{name}'");
            q.Execute();
        }

        public void DeleteQuery()
        {
            Query q = new Query(Database, $"DROP VIEW {Name}");
            q.Execute();
        }
    }
}
