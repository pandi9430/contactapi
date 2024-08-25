using UserDetailsAPI.Repository;
using DB.Common.Repository;
using NLog;
using ContactDetailsAPI.Data;
using ContactDetailsAPI.Models;

namespace UserDetailsAPI.Service
{
    public class UserService : IUserRepository<User, int,ResetPasswordFilter,RegisterFilter, UserStatusFilter>
    {
        private readonly ISqlDataAccessRepository _dataAccessRepository;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public UserService(ISqlDataAccessRepository dataAccessRepository)
        {
            _dataAccessRepository = dataAccessRepository;
        }

        public async Task<ApiResponse<IEnumerable<User>>> GetAllUserAsync()
        {
            var response = new ApiResponse<IEnumerable<User>>();
            try
            {
                var param = new { Type = "Get" };
                var result = await _dataAccessRepository.GetData<User>(StoredProcedures.Usermastersp, param);

                if (result != null && result.Any())
                {
                    response.Code = ResponseCode.Success;
                    response.Status = "SUCCESS";
                    response.Message = Messages.RecordFetchSuccess;
                    response.Result = result;
                }
                else
                {
                    response.Code = ResponseCode.NotFound;
                    response.Status = "SUCCESS";
                    response.Message = Messages.NoRecords;
                    response.Result = result;
                }
                _logger.Info("Fetched all Users successfully.");
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, "Error fetching Users.");
            }

            return response;
        }

        public async Task<ApiResponse<User>> GetUserByIdAsync(int id)
        {
            var response = new ApiResponse<User>();
            try
            {
                var param = new { Type = "Record", UserID = id };
                var result = await _dataAccessRepository.GetData<User>(StoredProcedures.Usermastersp, param);
                var User = result.FirstOrDefault();

                if (User != null)
                {
                    response.Code = ResponseCode.Success;
                    response.Status = "SUCCESS";
                    response.Message = Messages.RecordFetchSuccess;
                    response.Result = User;
                    _logger.Info($"Fetched User with ID {id} successfully.");
                }
                else
                {
                    response.Code = ResponseCode.NotFound;
                    response.Status = "SUCCESS";
                    response.Message = Messages.NoRecords;
                    response.Result = null;
                    _logger.Warn($"No User found with ID {id}.");
                }
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, $"Error fetching User with ID {id}.");
            }

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteUserAsync(int id)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var param = new { Type = "Delete", UserID = id };
                var result = await _dataAccessRepository.Execute(StoredProcedures.Usermastersp, param);

                response.Code = ResponseCode.Success;
                response.Status = "SUCCESS";
                response.Message = result > 0 ? Messages.RecordDelete : Messages.NoRecords;
                response.Result = result > 0;
                _logger.Info($"Deleted User with ID {id} successfully.");
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = false;
                _logger.Error(ex, $"Error deleting User with ID {id}.");
            }

            return response;
        }

        public async Task<ApiResponse<User>> InsertUserAsync(User record)
        {
            var response = new ApiResponse<User>();
            try
            {
                var param = new
                {
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email,
                    PhoneNumber = record.PhoneNumber,
                    Address = record.Address,
                    Country = record.Country,
                    State = record.State,
                    City = record.City,
                    PostalCode = record.PostalCode,
                    Password = record.Password,
                    Type = "Insert"
                };
                await _dataAccessRepository.Execute(StoredProcedures.Usermastersp, param);

                response.Result = record;
                response.Code = ResponseCode.Success;
                response.Status = "SUCCESS";
                response.Message = Messages.RecordAddSuccess;
                _logger.Info($"Inserted User successfully.");

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                _logger.Warn($"Failed to insert User.");
            }

            return response;
        }

        public async Task<ApiResponse<User>> UpdateUserAsync(User record)
        {
            var response = new ApiResponse<User>();
            if (record == null)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = "Record cannot be null";
                response.Result = null;
                return response;
            }

            try
            {
                var param = new
                {
                    UserID = record.UserID,
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email,
                    PhoneNumber = record.PhoneNumber,
                    Address = record.Address,
                    Country = record.Country,
                    State = record.State,
                    City = record.City,
                    PostalCode = record.PostalCode,
                    Password = record.Password,
                    Type = "Update"
                };
                await _dataAccessRepository.Execute(StoredProcedures.Usermastersp, param);

                response.Code = ResponseCode.Success;
                response.Status = "SUCCESS";
                response.Message = Messages.RecordUpdateSuccess;
                response.Result = record;
                _logger.Info($"Updated User with ID {record.UserID} successfully.");
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, $"Error updating User with ID {record.UserID}.");
            }

            return response;
        }

        public async Task<ApiResponse<User>> ResetPassword(ResetPasswordFilter record)
        {
            var response = new ApiResponse<User>();

            if (record == null)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = "Record cannot be null";
                response.Result = null;
                return response;
            }

            try
            {
                var param = new
                {
                    Email = record.Email,
                    Password = record.Password,
                    Type = "ResetPassword"
                };

                var result = await _dataAccessRepository.Execute(StoredProcedures.Usermastersp, param);

                if (result > 0)
                {
                    // Assuming you want to return the updated user object
                    var user = new User
                    {
                        Email = record.Email,
                        Password = record.Password
                    };

                    response.Code = ResponseCode.Success;
                    response.Status = "SUCCESS";
                    response.Message = Messages.PasswordResetSuccessful;
                    response.Result = user;
                    _logger.Info($"Password reset for user with Email {record.Email} successfully.");
                }
                else
                {
                    response.Code = ResponseCode.NotFound;
                    response.Status = "ERROR";
                    response.Message = "Failed to update password. User might not exist.";
                    response.Result = null;
                    _logger.Warn($"Failed to reset password for user with Email {record.Email}. User not found.");
                }
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, $"Error resetting password for user with Email {record.Email}.");
            }
            return response;
        }

        public async Task<ApiResponse<User>> RegisterUser(RegisterFilter record)
        {
            var response = new ApiResponse<User>();
            try
            {
                var param = new
                {
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email,
                    Password = record.Password,
                    Type = "Register"
                };
                await _dataAccessRepository.Execute(StoredProcedures.Usermastersp, param);

                response.Result = new User
                {
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email,
                    Password = record.Password
                };
                response.Code = ResponseCode.Success;
                response.Status = "SUCCESS";
                response.Message = Messages.RecordAddSuccess;
                _logger.Info($"Registered User with Email {record.Email} successfully.");
            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                _logger.Warn($"Failed to register User with Email {record.Email}.");
            }

            return response;
        }

        public async Task<ApiResponse<User>> UpdateUserStatus(UserStatusFilter record)
        {
            var response = new ApiResponse<User>();
            if (record == null)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = "Record cannot be null";
                response.Result = null;
                return response;
            }

            try
            {
                var param = new
                {
                    UserID = record.UserID,
                    UserStatus = record.UserStatus, // Assuming UserStatus is a property of UserStatusFilter
                    Type = "UpdateUserStatus"
                };

                await _dataAccessRepository.Execute(StoredProcedures.Usermastersp, param);

                var user = new User
                {
                    UserID = record.UserID,
                    // Assign other necessary properties, if any
                };

                response.Code = ResponseCode.Success;
                response.Status = "SUCCESS";
                response.Message = Messages.RecordUpdateSuccess;
                response.Result = user;
                _logger.Info($"Updated User status with ID {record.UserID} successfully.");
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, $"Error updating User status with ID {record.UserID}.");
            }

            return response;
        }

    }
}
