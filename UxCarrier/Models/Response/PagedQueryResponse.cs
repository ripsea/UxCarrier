using UxCarrier.Models.Dto;

namespace UxCarrier.Models.Response
{
    public class PagedQueryResponse
    {
        public PagedResult PagedResult { get; set; }
        public IEnumerable<QueryInvoiceDto>? PagedList { get; set; }
    }
}