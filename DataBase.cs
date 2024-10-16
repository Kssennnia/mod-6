using System;
using System.Data.SqlClient;

namespace Зад_4
{
    internal class DataBase
    {
        // Подключение к LocalDB
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=DESKTOP-8M799H7\SQLEXPRESS;Initial Catalog=mod6_zadanye4;Integrated Security=True");
        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }
        public void closedConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }
        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}