using FluentValidation;
using System;
using System.Text.Json;

namespace UxCarrier.Models.Dto
{
    public class EInvoiceBindStep4Dto
    {
        public string? card_ban { get; set; }
        public string? card_no1 { get; set; }
        public string? card_no2 { get; set; }
        public string? card_type { get; set; }
        public string? token { get; set; }
        public string? signature { get; set; }
        //public string? url { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}