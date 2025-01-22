using System.ComponentModel.DataAnnotations;

namespace UxCarrier.Models.Entities
{
    public class InvoiceCarrier
    {
        public int InvoiceID { get; set; }
        public string? CarrierType { get; set; }
        public string? CarrierNo { get; set; }
        public string? CarrierNo2 { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
    }
}