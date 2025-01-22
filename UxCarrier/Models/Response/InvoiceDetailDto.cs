namespace UxCarrier.Models.Response
{
    public class InvoiceDetailDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string PieceUnit { get; set; } = string.Empty;
        public string UnitCost { get; set; } = string.Empty;
        public string CostAmount { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
    }
}