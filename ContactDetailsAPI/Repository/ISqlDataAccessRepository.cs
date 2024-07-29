using ContactDetailsAPI.Models;

namespace DB.Common.Repository
{
    public interface ISqlDataAccessRepository
    {
        Task<IEnumerable<T>> GetData<T>(string storedProcedure, object parameters);
        Task<int> Execute(string storedProcedure, object parameters);
        Task<Login> ValidateUserAsync(string username, string password);
    }
}
