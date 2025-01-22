namespace UxCarrier.Models.Entities
{
    public class InvoiceSeller
    {
        public int InvoiceID { get; set; }
        public string ReceiptNo { get; set; }
        //public string PostCode { get; set; }
        //public string Address { get; set; }
        public string Name { get; set; }=string.Empty;
        //public int SellerID { get; set; }
        //public string CustomerID { get; set; }
        //public string ContactName { get; set; }
        //public string Phone { get; set; }
        //public string EMail { get; set; }
        //public string CustomerName { get; set; }
        //public string Fax { get; set; }
        //public string PersonInCharge { get; set; }
        //public string RoleRemark { get; set; }
        public InvoiceItem InvoiceItem { get; set; }
    }
}
