namespace StoreHub.Helpers
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiErrorResponse(int statusCode, string? message = null)
        {
            this.StatusCode = statusCode;
            this.Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A Bad Requset, You Have Made",
                401 => "You Are Not Authorized",
                404 => "Resoursed Not Found",
                500 => "There is a Server Error",
                _ => null,
            };
        }
    }
}