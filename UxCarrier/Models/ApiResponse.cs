using System.Net;
using System.Text.Json;

namespace UxCarrier.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object? Result { get; set; }
        public ApiResponse()
        {
            ErrorMessages = new List<string>();
            StatusCode = HttpStatusCode.OK;
            IsSuccess = true;
        }

        public ApiResponse ErrorMessage(string message = "something error.")
        {
            IsSuccess = false;
            ErrorMessages = new List<string>() { message };
            return this;
        }

        public ApiResponse ErrorMessage(List<string> errMessages)
        {
            IsSuccess = false;
            ErrorMessages = errMessages;
            return this;
        }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
