using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace UxCarrier.Models.Entities
{
    public class InvoiceItem
    {
        public int? InvoiceID { get; set; }
        public string? No { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string? CheckNo { get; set; }
        public string? Remark { get; set; }
        //public byte? BuyerRemark { get; set; }
        //public byte? CustomsClearanceMark { get; set; }
        //public string? TaxCenter { get; set; }
        //public DateTime? PermitDate { get; set; }
        //public string? PermitWord { get; set; }
        //public string? PermitNumber { get; set; }
        //public string? Category { get; set; }
        //public string? RelateNumber { get; set; }
        public byte? InvoiceType { get; set; }
        public string? GroupMark { get; set; }
        public string? DonateMark { get; set; }
        public int? SellerID { get; set; }
        //public int? DonationID { get; set; }
        public string? RandomNo { get; set; }
        public string? TrackCode { get; set; }
        //public byte? BondedAreaConfirm { get; set; }
        public string? PrintMark { get; set; }
        //public int? ProcessType { get; set; }
        public int? TrackID { get; set; }
        public InvoiceCarrier InvoiceCarrier { get; set; }
        public InvoiceSeller InvoiceSeller { get; set; }
        public InvoiceBuyer InvoiceBuyer { get; set; }
        public InvoiceAmountType InvoiceAmountType { get; set; }
        public InvoiceWinningNumber InvoiceWinningNumber { get; set; }
        public IEnumerable<InvoiceDetails> InvoiceDetails { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}