using AutoMapper.Internal;
using FluentValidation.Results;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using UxCarrier.Helper;
using UxCarrier.Models;
using UxCarrier.Services.IService;

namespace UxCarrier.Services
{
    public class BaseService : IBaseService
    {
        private readonly ILogger<UxBindService> _logger;
        public ApiResponse responseModel { get; set; }
        public IHttpClientFactory httpClientFactory { get; set; }
        public ApiResponse ResponseModel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BaseService(IHttpClientFactory httpClient, ILogger<UxBindService> logger)
        {
            this.responseModel = new();
            this.httpClientFactory = httpClient;
            _logger = logger;
        }

        public static string GetFluentValidationResult(ValidationResult validationResult)
        {
            bool result = validationResult.IsValid;
            if (result)
            {
                return string.Empty;
            }
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage);
            var resultMessage = string.Join(",", errorMessages);
            return resultMessage;
        }

        /// <summary>
        /// 發送 POST 請求
        /// </summary>
        /// <typeparam name="TRequest">傳入的資料型別</typeparam>
        /// <typeparam name="TResponse">回應的資料型別</typeparam>
        /// <param name="url">API URL</param>
        /// <param name="data">傳入的資料</param>
        /// <returns>回傳的資料型別</returns>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var _httpClient = httpClientFactory.CreateClient("HttpSend");
            // 將傳入資料序列化為 JSON
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // 發送 POST 請求
            var response = await _httpClient.PostAsync(url, content);

            // 確保成功的 HTTP 回應狀態
            response.EnsureSuccessStatusCode();

            // 讀取並反序列化回應內容
            var responseContent = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TResponse>(responseContent);
        }
    }
}
