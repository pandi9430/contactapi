using ContactDetailsAPI.Data;

namespace UserDetailsAPI.Repository
{
    public interface IUserRepository<T, U,M,B,J>
    {
        Task<ApiResponse<IEnumerable<T>>> GetAllUserAsync();
        Task<ApiResponse<T>> GetUserByIdAsync(U id);
        Task<ApiResponse<bool>> DeleteUserAsync(U id);
        Task<ApiResponse<T>> InsertUserAsync(T record);
        Task<ApiResponse<T>> RegisterUser(B record);
        Task<ApiResponse<T>> UpdateUserAsync(T record);
        Task<ApiResponse<T>> ResetPassword(M record);
        Task<ApiResponse<T>> UpdateUserStatus(J record);
    }
}
