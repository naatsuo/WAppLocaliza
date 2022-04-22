using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAppLocaliza.Entities
{
    public class CarHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public float PenaltyPrice { get; set; }
        [Required]
        public CarCheckList CheckList { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string Note { get; set; }
    }
}

