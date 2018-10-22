using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace SSMSLite
{
    public class FolderColumns : Folder
    {
        public FolderColumns(ColumnCollection columnCollection, string name = "Columns") : base (name)
        {
            ColumnCollection = columnCollection;
        }
        public override void LoadData()
        {
            Objects.Clear();
            SqlConnection conn = new SqlConnection(Database.ConnectionString);
            SqlCommand cmd = new SqlCommand(@"SELECT distinct c.COLUMN_NAME, c.IS_NULLABLE, c.DATA_TYPE, ISNULL(c.CHARACTER_MAXIMUM_LENGTH, COL_LENGTH(c.TABLE_NAME, c.COLUMN_NAME)), 
                                                case when t.COLUMN_NAME = c.COLUMN_NAME AND t.CONSTRAINT_NAME like 'PK_%' then 'YES' else 'NO' end as PRIMARY_KEY
                                                FROM INFORMATION_SCHEMA.COLUMNS c
                                                left join INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE t on t.TABLE_NAME = c.TABLE_NAME AND c.COLUMN_NAME = t.COLUMN_NAME
                                                WHERE c.TABLE_NAME = '" + ColumnCollection.Name+"'", conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var c = new Column(ColumnCollection)
                {
                    Name = reader.GetString(0),
                    Nullable = reader.GetString(1) == "YES",
                    DataType = reader.GetString(2),
                    MaxLength = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                    PrimaryKey = reader.GetString(4) == "YES"
                };
                Objects.Add(c);
            }
            conn.Close();
        }
    }
}
