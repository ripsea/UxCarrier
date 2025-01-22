using System.Linq;
using UxCarrier.Models.Response;

namespace UxCarrier.Models.Dto
{
    public class QueryInvoiceDto
    {
        public string SellerName { get; set; } = string.Empty;
        public string SellerReceiptNo { get; set; } = string.Empty;
        public string InvoiceNo { get; set; } = string.Empty;
        public DateTime? InvoiceDate { get; set; }
        public string? BuyerName { get; set; } = string.Empty;
        public string? BuyerReceiptNo { get; set; } = string.Empty;
        public string? Currency { get; set; }
        public string? SaleAmount { get; set; }
        public string? TaxType { get; set; } = "應稅";
        public string? TaxAmount { get; set; }
        public string? TotalAmount { get; set; }
        public string? Remark => String.Join("", InvoiceDetail.SelectMany(x => x.Remark).ToArray());
        public string? PrintMark { get; set; }
        public string? CarrierNo { get; set; }
        public string? HasBonus { get; set; }
        public string? DonateMark { get; set; }
        public string? CustomerID { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CarrierType { get; set; }
        public string? CheckNo { get; set; }
        public string? RandomNo { get; set; }
        public IEnumerable<InvoiceDetailDto> InvoiceDetail { get; set; }
    }
}
