using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAppLocaliza.Entities
{
    public class CarModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Description { get; set; }
        public ICollection<Car> Cars { get; set; }
        public CarBrand Brand { get; set; }

    }
}

