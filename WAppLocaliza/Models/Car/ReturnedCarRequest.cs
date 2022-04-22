using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class ReturnedCarRequest
    {
        [Required]
        public Guid ScheduleId { get; set; }
        [Required]
        public DateTime? ReturnedAt { get; set; }
        [Required]
        public bool Clean { get; set; }
        [Required]
        public bool FuelTankFull { get; set; }
        [Required]
        public bool Dented { get; set; }
        [Required]
        public bool Scratched { get; set; }
        public string Note { get; set; }

    }
}