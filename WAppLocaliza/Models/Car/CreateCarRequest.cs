using System.ComponentModel.DataAnnotations;
using WAppLocaliza.Entities;

namespace WAppLocaliza.Models
{
    public class CreateCarRequest
    {
        [Required]
        public Guid ModelId { get; set; }
        [Required]
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
    }
}