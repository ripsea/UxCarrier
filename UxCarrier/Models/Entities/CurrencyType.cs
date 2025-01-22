namespace UxCarrier.Models.Entities
{
    public class CurrencyType
    {
        public int CurrencyID { get; set; }
        public string CurrencyName { get; set; }
        //public string AbbrevName { get; set; }
        //public string FormatPattern { get; set; }
        //public short Decimals { get; set; }
        public InvoiceAmountType InvoiceAmountType { get; set; }
    }
}
