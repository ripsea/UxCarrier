using System.ComponentModel.DataAnnotations;

namespace UxCarrier.Models.Entities
{
    public class InvoiceDetails
    {
        public int InvoiceID { get; set; }
        public int ProductID { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
        public InvoiceProduct InvoiceProduct { get; set; }
        public InvoiceProductItem InvoiceProductItem { get; set; }

    }
}