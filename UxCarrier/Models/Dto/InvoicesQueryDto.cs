using UxCarrier.Models.Request;

namespace UxCarrier.Models.Dto
{
    public class InvoicesQueryDto
    {
        public PageInfo? PageInfo { get; set; }
        public string CarrierNo { get; set; }
    }
}
