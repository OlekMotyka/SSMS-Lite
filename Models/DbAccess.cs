using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public static class DbAccess
    {
        public static object ExecuteScalar(string connectionString, string query)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            var res = cmd.ExecuteScalar();
            conn.Close();
            return res;
        }

        public static DataTable ExecuteReader(string connectionString, string query)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            var res = new DataTable();
            res.Load(reader);
            conn.Close();
            return res;
        }
    }
}
