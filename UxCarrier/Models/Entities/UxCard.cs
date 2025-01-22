using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UxCarrier.Models.Entities
{
    public class UxCard
    {
        [Key]
        [Column(TypeName = "VARCHAR")]
        [StringLength(64)]
        public string CardNo { get; set; }
        [Column(TypeName = "BIT")]
        public bool UxBind { get; set; }
        [Column(TypeName = "BIT")]
        public bool? EInvoiceBind { get; set; }
        
        public List<UxCardEmail>? MemberEmails { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}