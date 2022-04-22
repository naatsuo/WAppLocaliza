using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAppLocaliza.Entities
{
    public enum FuelType
    {
        Gasoline,
        Alcohol,
        Diesel,
        Electric,
        Hybrid,
    }

    public enum CategoryType
    {
        Basic,
        Complete,
        Deluxe
    }

    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(24)")]
        public string Plate { get; set; }
        [Required]
        public int Year { get; set; }
        [Required]
        public float PriceHour { get; set; }
        [Required]
        public FuelType Fuel { get; set; }
        [Required]
        public int TrunkLimit { get; set; }
        [Required]
        public CategoryType Category { get; set; }
        [Required]
        public float PercentagePenalty { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Column(TypeName = "VARCHAR(MAX)")]
        public string Note;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public ICollection<CarHistory> Histories { get; set; }
        public CarModel Model { get; set; }
    }
}

