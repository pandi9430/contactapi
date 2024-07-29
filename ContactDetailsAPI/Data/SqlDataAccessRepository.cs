using ContactDetailsAPI.Data;
using ContactDetailsAPI.Models;
using Dapper;
using DB.Common.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

public class SqlDataAccessRepository : ISqlDataAccessRepository
{
    private readonly ApplicationDbContext _dbContext;

    public SqlDataAccessRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<T>> GetData<T>(string storedProcedure, object parameters)
    {
        using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
        {
            await connection.OpenAsync();
            var result = await connection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }


    public async Task<int> Execute(string storedProcedure, object parameters)
    {
        try
        {
            using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var property in parameters.GetType().GetProperties())
                        {
                            var value = property.GetValue(parameters, null);
                            var paramName = "@" + property.Name;
                            command.Parameters.AddWithValue(paramName, value ?? DBNull.Value);
                        }
                    }
                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"Stored procedure executed. Rows affected: {rowsAffected}");
                    return rowsAffected;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw;
        }
    }
    public async Task<Login> ValidateUserAsync(string username, string password)
    {
        using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand("SP_ValidateUser", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Login
                        {
                            UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            Role = reader.GetString(reader.GetOrdinal("Role"))
                        };
                    }
                }
            }
        }

        return null;
    }
}
