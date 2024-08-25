using ContactDetailsAPI.Data;
using ContactDetailsAPI.Repository;
using DB.Common.Repository;
using NLog;

namespace ContactDetailsAPI.Service
{
    public class AuthService : IAuthRepository<LoginDTO>
    {
        private readonly ISqlDataAccessRepository _dataAccessRepository;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public AuthService(ISqlDataAccessRepository dataAccessRepository)
        {
            _dataAccessRepository = dataAccessRepository;
        }

        public async Task<ApiResponse<LoginDTO>> UserAuth(LoginDTO record)
        {
            var response = new ApiResponse<LoginDTO>();
            try
            {
                var param = new
                {
                    UserName = record.Email,
                    Password = record.Password
                };

                // Assuming your stored procedure returns a result set indicating if the user exists and is valid
                var result = await _dataAccessRepository.GetData<dynamic>(StoredProcedures.LoginUser, param);

                // Check if any result was returned
                if (result != null && result.Any())
                {
                    var user = result.FirstOrDefault();
                    if (user != null)
                    {
                        // Extract role information from dynamic result
                        var roleName = (string)user.RoleName; // Ensure the correct property name
                        response.Code = ResponseCode.Success;
                        response.Status = "SUCCESS";
                        response.Message = "User authenticated successfully.";
                        response.Result = new LoginDTO
                        {
                            Role_Name = roleName  // Set the role name
                        };
                        _logger.Info($"User {record.Email} authenticated successfully.");
                    }
                    else
                    {
                        response.Code = ResponseCode.Unauthorized;
                        response.Status = "FAILED";
                        response.Message = "Invalid email or password.";
                        response.Result = null;
                        _logger.Warn($"Failed to authenticate user {record.Email}.");
                    }
                }
                else
                {
                    response.Code = ResponseCode.Unauthorized;
                    response.Status = "FAIL";
                    response.Message = "Invalid email or password.";
                    response.Result = null;
                    _logger.Warn($"Failed to authenticate user {record.Email}.");
                }
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"Authentication failed: {ex.Message}";
                _logger.Error(ex, "Error during user authentication.");
            }
            return response;
        }
    }
}
