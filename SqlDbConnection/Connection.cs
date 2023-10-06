using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace NimapInfotechMachineTest.SqlDbConnection
{
    public class Connection
    {
        SqlCommand cmd;
        SqlDataAdapter da;
        SqlConnection conn = null;
        public static string connectionString = @"Data Source=DESKTOP-6736EHK; Initial Catalog=Db_NimapInfotech ; User Id=sa;Password=Game@123";

        public SqlConnection Connect()
        {
            try
            {

                conn = new SqlConnection(connectionString);
                conn.Close();
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                conn.Open();
            }
            catch (Exception ex)
            {

            }

            return conn;
        }
        public DataTable FillCombo(string query)
        {
            DataTable dt = new DataTable();

            conn = Connect();
            cmd = new SqlCommand();
            cmd.Connection = conn;
            da = new SqlDataAdapter(query, conn);
            da.Fill(dt);


            return dt;
        }
    }
}