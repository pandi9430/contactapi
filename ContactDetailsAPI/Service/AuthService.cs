using ContactDetailsAPI.Data;
using ContactDetailsAPI.Repository;
using ContactDetailsAPI.Token;
using DB.Common.Repository;

public class AuthService : IAuthRepository
{
    private readonly ISqlDataAccessRepository _dataAccessRepository;
    private readonly TokenService _tokenService;

    public AuthService(ISqlDataAccessRepository dataAccessRepository, TokenService tokenService)
    {
        _dataAccessRepository = dataAccessRepository;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<string>> AuthenticateAsync(string username, string password)
    {
        var user = await _dataAccessRepository.ValidateUserAsync(username, password);

        if (user == null)
        {
            return new ApiResponse<string>
            {
                Code = ResponseCode.Failure,
                Status = "ERROR",
                Message = "Invalid credentials",
                Result = null
            };
        }

        var token = _tokenService.GenerateToken(user.Username, user.Role);
        return new ApiResponse<string>
        {
            Code = ResponseCode.Success,
            Status = "SUCCESS",
            Message = "Authentication successful",
            Result = token
        };
    }
}
