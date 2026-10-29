
using Microsoft.Data.SqlClient;

namespace DAL
{
    public static class Database
    {
        private static readonly string connectionString =
            @"Server=.\SQLEXPRESS;Database=HotelDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
