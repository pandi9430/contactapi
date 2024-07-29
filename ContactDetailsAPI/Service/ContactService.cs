using ContactDetailsAPI.Data;
using ContactDetailsAPI.Models;
using ContactDetailsAPI.Repository;
using DB.Common.Repository;
using NLog;

namespace ContactDetailsAPI.Service
{
    public class ContactService : IContactRepository<Contact, int>
    {
        private readonly ISqlDataAccessRepository _dataAccessRepository;
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public ContactService(ISqlDataAccessRepository dataAccessRepository)
        {
            _dataAccessRepository = dataAccessRepository;
        }

        public async Task<ApiResponse<IEnumerable<Contact>>> GetAllContactAsync()
        {
            var response = new ApiResponse<IEnumerable<Contact>>();
            try
            {
                var param = new { Type = "Get" };
                var result = await _dataAccessRepository.GetData<Contact>(StoredProcedures.contactmastersp, param);

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
                _logger.Info("Fetched all contacts successfully.");
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, "Error fetching contacts.");
            }

            return response;
        }

        public async Task<ApiResponse<Contact>> GetContactByIdAsync(int id)
        {
            var response = new ApiResponse<Contact>();
            try
            {
                var param = new { Type = "Record", ContactID = id };
                var result = await _dataAccessRepository.GetData<Contact>(StoredProcedures.contactmastersp, param);
                var contact = result.FirstOrDefault();

                if (contact != null)
                {
                    response.Code = ResponseCode.Success;
                    response.Status = "SUCCESS";
                    response.Message = Messages.RecordFetchSuccess;
                    response.Result = contact;
                    _logger.Info($"Fetched contact with ID {id} successfully.");
                }
                else
                {
                    response.Code = ResponseCode.NotFound;
                    response.Status = "SUCCESS";
                    response.Message = Messages.NoRecords;
                    response.Result = null;
                    _logger.Warn($"No contact found with ID {id}.");
                }
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, $"Error fetching contact with ID {id}.");
            }

            return response;
        }

        public async Task<ApiResponse<bool>> DeleteContactAsync(int id)
        {
            var response = new ApiResponse<bool>();
            try
            {
                var param = new { Type = "Delete", ContactID = id };
                var result = await _dataAccessRepository.Execute(StoredProcedures.contactmastersp, param);

                response.Code = ResponseCode.Success;
                response.Status = "SUCCESS";
                response.Message = result > 0 ? Messages.RecordDelete : Messages.NoRecords;
                response.Result = result > 0;
                _logger.Info($"Deleted contact with ID {id} successfully.");
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = false;
                _logger.Error(ex, $"Error deleting contact with ID {id}.");
            }

            return response;
        }

        public async Task<ApiResponse<Contact>> InsertContactAsync(Contact record)
        {
            var response = new ApiResponse<Contact>();
            try
            {
                var param = new
                {
                    FirstName=record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email,
                    PhoneNumber = record.PhoneNumber,
                    Address = record.Address,
                    Country = record.Country,
                    State = record.State,
                    City = record.City,
                    PostalCode = record.PostalCode,
                    Type = "Insert"
                };
                await _dataAccessRepository.Execute(StoredProcedures.contactmastersp, param);

                response.Result = record;
                response.Code = ResponseCode.Success;
                response.Status = "SUCCESS";
                response.Message = Messages.RecordAddSuccess;
                _logger.Info($"Inserted contact successfully.");

            }
            catch (Exception ex)
            {
                response.Result = null;
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                _logger.Warn($"Failed to insert contact.");
            }

            return response;
        }

        public async Task<ApiResponse<Contact>> UpdateContactAsync(Contact record)
        {
            var response = new ApiResponse<Contact>();
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
                    ContactID = record.ContactID,
                    FirstName = record.FirstName,
                    LastName = record.LastName,
                    Email = record.Email,
                    PhoneNumber = record.PhoneNumber,
                    Address = record.Address,
                    Country = record.Country,
                    State = record.State,
                    City = record.City,
                    PostalCode = record.PostalCode,
                    Type = "Update"
                };
                await _dataAccessRepository.Execute(StoredProcedures.contactmastersp, param);

                response.Code = ResponseCode.Success;
                response.Status = "SUCCESS";
                response.Message = Messages.RecordUpdateSuccess;
                response.Result = record;
                _logger.Info($"Updated contact with ID {record.ContactID} successfully.");
            }
            catch (Exception ex)
            {
                response.Code = ResponseCode.Failure;
                response.Status = "ERROR";
                response.Message = $"{Messages.RecordFetchFailed}: {ex.Message}";
                response.Result = null;
                _logger.Error(ex, $"Error updating contact with ID {record.ContactID}.");
            }

            return response;
        }
    }
}
