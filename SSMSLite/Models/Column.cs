using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Column : DbObject
    {        
        public string DataType { get; set; }
        public int MaxLength { get; set; }
        public bool Nullable { get; set; }
        public bool PrimaryKey { get; set; }

        public Column(DbObject parent, string name) : base(parent, name)
        {
        }

        public string CoumnNameWithData
        {
            get
            {
                string pk = PrimaryKey ? "PRIMARY KEY, " : "";
                string nl = Nullable ? "NULL" : "NOT NULL";
                return $"{Name} {DataType}({MaxLength}) {nl} {pk})";
            }
        }

        public void Add()
        {
            if (Parent is Table)
                (Parent as Table).AddColumn();
        }

        public void Edit()
        {
            var pk = PrimaryKey ? " PRIMARY KEY" : "";
            var nl = Nullable ? " NULL" : " NOT NULL";
            var q = new Query(Database, $"ALTER TABLE {Parent.Name}\nALTER COLUMN {Name} {DataType}({MaxLength}){nl}{pk}");
            Database.Instance.AddQuery(q);
        }
        
        public void Rename(string name)
        {
            var q = new Query(Database, $"EXEC sp_rename '{Database.Name}.{Parent.Name}.{Name}', '{name}'", "Rename column");
            q.Execute();
        }

        public void Delete()
        {
            var q = new Query(Database, $"ALTER TABLE {Parent.Name} DROP COLUMN {Name}");
            q.Execute();
        }

        public override string ToString()
        {
            string pk = PrimaryKey ? "PK, " : "";
            string nl = Nullable ? "null" : "not null";
            return $"{Name} ({pk}{DataType}, {nl})";
        }
    }
}
