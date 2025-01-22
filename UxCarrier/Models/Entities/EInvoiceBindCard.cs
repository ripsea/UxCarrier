namespace UxCarrier.Models.Entities
{
    public class EInvoiceBindCard
    {
        public int Id { get; set; }
        public string card_ban { get; set; } = string.Empty;
        public string card_no1 { get; set; } = string.Empty;
        public string card_no2 { get; set; } = string.Empty;
        public string card_type { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public string nonce { get; set; } = string.Empty;
        public string ban { get; set; } = string.Empty;
        public bool token_flag { get; set; }
        public string err_msg { get; set; } = string.Empty;
        public string signature { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateTokenFlagDateTime { get; set; }
        public DateTime BindSendDateTime { get; set; }
    }
}