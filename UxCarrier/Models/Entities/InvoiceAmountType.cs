namespace UxCarrier.Models.Entities
{
    public class InvoiceAmountType
    {
        public int InvoiceID { get; set; }
        public byte TaxType { get; set; }
        public decimal SalesAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TotalAmount { get; set; }
        //public string TotalAmountInChinese { get; set; }
        //public decimal DiscountAmount { get; set; }
        //public decimal Adjustment { get; set; }
        //public decimal OriginalCurrencyAmount { get; set; }
        //public decimal ExchangeRate { get; set; }
        public int CurrencyID { get; set; }
        //public decimal FreeTaxSalesAmount { get; set; }
        //public decimal ZeroTaxSalesAmount { get; set; }
        //public byte BondedAreaConfirm { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
        public CurrencyType CurrencyType { get; set; }  
    }
}
