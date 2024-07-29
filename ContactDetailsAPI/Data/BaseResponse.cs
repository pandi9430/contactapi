namespace ContactDetailsAPI.Data
{
    public class BaseResponse
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
    }

    public class ApiResponse<T> : BaseResponse
    {
        public T Result { get; set; }
    }

}
