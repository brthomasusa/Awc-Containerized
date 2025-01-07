#pragma warning disable RCS1194

using WebUI.Models;

namespace WebUI.Exceptions
{
    public sealed class ApiResponseException(ApiErrorResponse errorDetails) : Exception(errorDetails.Message)
    {
        public ApiErrorResponse ErrorDetails { get; set; } = errorDetails;
    }
}