using Microsoft.Data.SqlClient;
using System;
using System.Threading.Tasks;

public static class DatabaseConnectionTest
{
    // Method to test database connection
    public static async Task TestConnectionAsync(string connectionString)
    {
        try
        {
            await using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                Console.WriteLine("Database connection successful.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Database connection failed: {ex.Message}");
        }
    }
}
