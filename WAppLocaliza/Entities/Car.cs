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

    public class CarBrand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(100)")]
        public string Name { get; set; }
        public ICollection<CarBrand> Models { get; set; }
    }

    public class CarModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Description { get; set; }
        public ICollection<Car> Cars { get; set; }
        public CarBrand Brand { get; set; }

    }


    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public string Plate { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }
        public string PriceHour { get; set; }
        public FuelType Fuel { get; set; }
        public string TrunkLimit { get; set; }
        public CategoryType Category { get; set; } 
        public DateTime CreatedAt { get; set; }

        public CarModel Model { get; set; }

    }
}

