using ContactDetailsAPI.Data;

namespace ContactDetailsAPI.Repository
{
    public interface IAuthRepository
    {
        Task<ApiResponse<string>> AuthenticateAsync(string username, string password);
    }
}
