using UxCarrier.Services.Validation;

namespace UxCarrier.Models.Request
{
    public class CaptchaRequest
    {
        [CaptchaValidation("EncryptedCode", ErrorMessage = "驗證碼錯誤！")]
        public string ValidCode { get; set; } = string.Empty;

        public string EncryptedCode { get; set; } = string.Empty;
    }
}