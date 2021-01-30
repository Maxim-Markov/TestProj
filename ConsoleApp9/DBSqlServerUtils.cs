using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace ConsoleApp9
{
    
    class DBSQLServerUtils
    {
       
        public static SqlConnection GetDBConnection()
        {
            string connString = @"Data Source=МАКСИМПК;Initial Catalog=userdb;Integrated Security=True";

            SqlConnection conn = new SqlConnection(connString);

            return conn;
        }


    }
}