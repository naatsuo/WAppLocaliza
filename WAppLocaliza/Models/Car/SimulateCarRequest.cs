using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class SimulateCarRequest
    {
        [Required]
        public Guid CarId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
    }
}