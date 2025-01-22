using System.Text.Json;

namespace UxCarrier.Models.Dto
{
    public class UxBindStep4Dto
    {
        public string? card_ban { get; set; }
        public string? card_no1 { get; set; }
        public string? card_no2 { get; set; }
        public string? card_type { get; set; }
        public string? rtn_flag { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
