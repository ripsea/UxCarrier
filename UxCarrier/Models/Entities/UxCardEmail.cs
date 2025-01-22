using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UxCarrier.Models.Entities
{
    public class UxCardEmail
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }
        [Key]
        [Column(TypeName = "VARCHAR")]
        [StringLength(128)]
        public string Email { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(64)]
        public string? CardNo { get; set; }
        [DataType(DataType.Date)]
        public DateTime LastOtpCodeRequestDateTime { get; set; }
        public UxCard? UxCard { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}