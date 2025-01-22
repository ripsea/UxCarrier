using System.Text.Json;

namespace UxCarrier.Models.Dto
{
    public class UxBindStep2Dto
    {
        public string? card_ban { get; set; }
        public string? card_no1 { get; set; }
        public string? card_no2 { get; set; }
        public string? card_type { get; set; }
        public string? back_uri { get; set; }
        public string? token { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
