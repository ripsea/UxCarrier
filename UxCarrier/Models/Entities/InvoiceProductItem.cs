namespace UxCarrier.Models.Entities
{
    public class InvoiceProductItem
    {
        public int ItemID { get; set; }
        public int ProductID { get; set; }
        public short No { get; set; }
        //public string? Spec { get; set; }
        public decimal Piece { get; set; }
        //public decimal Piece2 { get; set; }
        public string? PieceUnit { get; set; }
        //public string? PieceUnit2 { get; set; }
        //public decimal Weight { get; set; }
        //public string WeightUnit { get; set; } = string.Empty;
        //public decimal UnitFreight { get; set; }
        public decimal UnitCost { get; set; }
        //public decimal UnitCost2 { get; set; }
        //public decimal FreightAmount { get; set; }
        public decimal CostAmount { get; set; }
        //public decimal CostAmount2 { get; set; }
        //public decimal OriginalPrice { get; set; }
        public string Remark { get; set; } = string.Empty;
        //public string RelateNumber { get; set; } = string.Empty;
        public byte TaxType { get; set; }
        //public string ItemNo { get; set; }=string.Empty;
        public InvoiceDetails InvoiceDetails { get; set; }
    }
}
