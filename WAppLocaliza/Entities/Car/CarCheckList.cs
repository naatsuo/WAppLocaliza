using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAppLocaliza.Entities
{
    public class CarCheckList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public bool Clean { get; set; }
        [Required]
        public bool FuelTankFull { get; set; }
        [Required]
        public bool Dented { get; set; }
        [Required]
        public bool Scratched { get; set; }
    }
}

