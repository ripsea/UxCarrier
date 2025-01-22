using System.ComponentModel.DataAnnotations;

namespace UxCarrier.Models.Entities
{
    public class InvoiceProduct
    {
        public int ProductID { get; set; }
        public string Brief { get; set; }
        public InvoiceDetails InvoiceDetails { get; set; }
    }
}