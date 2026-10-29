using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class Database
    {
        private static readonly string connectionString =
            @"Server=DESKTOP-TSRKH98\SQLEXPRESS;Database=InventoryDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}