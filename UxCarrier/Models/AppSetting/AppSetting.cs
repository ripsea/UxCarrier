using System.Reflection.Metadata.Ecma335;

namespace UxCarrier.Models.AppSetting
{
    public class AppSetting: IAppSetting
    {
        public string? UxBindCallBack { get; set; }
        public string? UxBindUrl { get; set; }
        public string? UxBindCardBan { get; set; }
        public string? UxBindCardType { get; set; }
        public string? EInvoiceBindStep2Url { get; set; }
        public string? EInvoiceBindStep4Url { get; set; }
        public string? EInvoiceBindSecretKey { get; set; }
        public string? JwtSecret { get; set; }
        public string? BasePath { get; set; }
        public string? ApiVersion { get; set; }
        public IEnumerable<string>? DevOpList { get; set; }
        public string? DevTestCardNo { get; set; }
        public string? DevTestEmail { get; set; }
    }
}
