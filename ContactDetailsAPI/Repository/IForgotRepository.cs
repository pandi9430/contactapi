using ContactDetailsAPI.Data;

namespace ContactDetailsAPI.Repository
{
    public interface IForgotRepository<T,U>
    {
        Task<ApiResponse<T>> GetUserCheck(U id);
    }
}
