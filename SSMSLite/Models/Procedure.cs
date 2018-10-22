using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Models
{
    public class Procedure : DbObject
    {
        public Procedure(DbObject parent, string name) : base(parent, name)
        {
        }


        public void New()
        {
            Database.NewProcedure();
        }

        public void Design()
        {
            var q = (string)DbAccess.ExecuteScalar(Database.ConnectionString, $"SELECT ROUTINE_DEFINITION FROM [{Database.Name}].[INFORMATION_SCHEMA].[ROUTINES] where ROUTINE_SCHEMA = '{Schema}' AND ROUTINE_NAME = '{Name}'");
            q = new Regex("CREATE").Replace(q, "ALTER", 1);
            Database.Instance.AddQuery(new Query(Database, q, $"ALTER {Name}"));
        }

        public void Execute()
        {
            Database.Instance.AddQuery(new Query(Database, @"DECLARE	@return_value int

EXEC	@return_value = [" + Schema + "].[" + Name + @"]
		<@parameter = value>,
        <@parameter = value>

SELECT	'Return Value' = @return_value", $"EXEC {Name}"));
        }

        public void Rename(string name)
        {
            var q = new Query(Database, $"EXEC sp_rename '{Database.Name}.{Schema}.{Name}', '{name}'", "Rename procedure");
            q.Execute();
        }

        public void Delete()
        {
            Query q = new Query(Database, $"DROP PROCEDURE {Name}");
            q.Execute();
        }
    }

    public class Function : Procedure
    {
        public string Returns { get; set; }

        public Function(DbObject parent, string name) : base(parent, name)
        {
        }

    }
}
