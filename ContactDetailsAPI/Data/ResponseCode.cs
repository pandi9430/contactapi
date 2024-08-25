namespace ContactDetailsAPI.Data
{
    public static class ResponseCode
    {
        public const string Success = "200";            // OK
        public const string Failure = "500";            // Internal Server Error
        public const string NotFound = "404";           // Not Found
        public const string Accepted = "202";           // Accepted
        public const string Unauthorized = "401";       // Unauthorized
        public const string Forbidden = "403";          // Forbidden
    }
}
