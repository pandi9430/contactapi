using ContactDetailsAPI.Data;

namespace ContactDetailsAPI.Repository
{
    public interface IAuthRepository<M>
    {
        Task<ApiResponse<M>> UserAuth(M record); // Changed to return ApiResponse<M>
    }
}
