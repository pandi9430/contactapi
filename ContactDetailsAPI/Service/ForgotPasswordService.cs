using ContactDetailsAPI.Data;
using ContactDetailsAPI.Models;
using ContactDetailsAPI.Repository;
using DB.Common.Repository;
using NLog;

namespace ContactDetailsAPI.Service
{
    public class ForgotPasswordService : IForgotRepository<ForgotPassword, string>
    {
        private readonly ISqlDataAccessRepository _dataAccessRepository;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public ForgotPasswordService(ISqlDataAccessRepository dataAccessRepository)
        {
            _dataAccessRepository = dataAccessRepository;
        }

        public async Task<ApiResponse<ForgotPassword>> GetUserCheck(string email)
        {
            var response = new ApiResponse<ForgotPassword>();
            try
            {
                var param = new { Type = "ForgotPassword", Email = email };
                var result = await _dataAccessRepository.GetData<ForgotPassword>(StoredProcedures.Usermastersp, param);

                if (result != null && result.Any())
                {
                    var user = result.FirstOrDefault();

                    if (user != null)
                    {
                        response.Code = ResponseCode.Success;
                        response.Status = "SUCCESS";
                        response.Message = Messages.RecordFetchSuccess;
                        //response.Result = user;
                        _logger.Info($"Fetched User with Email {email} successfully.");
                    }
                    else
                    {
                        response.Code = ResponseCode.NotFound;
                        response.Status = "SUCCESS";
                        response.Message = Messages.NoRecords;
                        response.Result = null;
                        _logger.Warn($"No User found with Email {email}.");
                    }
                }
                else
                {
                    response.Code = ResponseCode.NotFound;
                    response.Status = "SUCCESS";
                    response.Message = Messages.NoRecords;
                    response.Result = null;
                    _logger.Warn($"No User found with Email {email}.");
                }
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, $"Error fetching User with Email {email}.");
            }

            return response;
        }
    }
}
