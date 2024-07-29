using ContactDetailsAPI.Data;
using ContactDetailsAPI.Models;

namespace ContactDetailsAPI.Repository
{
    public interface IContactRepository<T,U>
    {
        Task<ApiResponse<IEnumerable<T>>> GetAllContactAsync();
        Task<ApiResponse<T>> GetContactByIdAsync(U id);
        Task<ApiResponse<bool>> DeleteContactAsync(U id);
        Task<ApiResponse<T>> InsertContactAsync(T record);
        Task<ApiResponse<T>> UpdateContactAsync(T record);
    }
}
