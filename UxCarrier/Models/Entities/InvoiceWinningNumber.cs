namespace UxCarrier.Models.Entities
{
    public class InvoiceWinningNumber
    {
        public int InvoiceID { get; set; }
        //public int WinningID { get; set; }
        //public DateTime DownloadDate { get; set; }
        //public string PrizeType { get; set; }
        //public int Bonus { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
    }
}
