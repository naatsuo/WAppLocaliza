using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class ScheduleCarRequest
    {
        [Required]
        public Guid CarId { get; set; }
        [Required]
        public Guid ClientId { get; set; }
        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        public string Note { get; set; }
    }
}