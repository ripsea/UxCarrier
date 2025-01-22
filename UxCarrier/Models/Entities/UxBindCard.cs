using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
namespace UxCarrier.Models.Entities
{
    public class UxBindCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string? card_ban { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(64)]
        public string card_no1 { get; set; }=string.Empty;
        [Column(TypeName = "VARCHAR")]
        [StringLength(64)]
        public string? card_no2 { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(16)]
        public string? card_type { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(256)]
        public string? back_url { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(256)]
        public string? token { get; set; }
        //[Column(TypeName = "VARCHAR")]
        //[StringLength(64)]
        //public string? Step1Result { get; set; }
        [Column(TypeName = "VARCHAR")]
        [StringLength(8)]
        public string? Step2Result { get; set; }
        //[Column(TypeName = "VARCHAR")]
        //[StringLength(8)]
        //public string? Step4Result { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public override string? ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
