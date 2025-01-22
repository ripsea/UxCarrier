using UxCarrier.Models;

namespace UxCarrier.Services.IService
{
    public interface IBaseService
    {
        ApiResponse ResponseModel { get; set; }
        Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data);
    }
}
