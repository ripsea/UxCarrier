

using UxCarrier.Helper;

namespace UxCarrier.Models
{
    public class ApiRequest
    {
        public Utilities.ApiType ApiType { get; set; } = Utilities.ApiType.GET;
        public string? Url { get; set; }
        public object? Data { get; set; }
    }
}
