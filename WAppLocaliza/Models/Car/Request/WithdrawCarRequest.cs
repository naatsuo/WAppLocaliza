using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class WithdrawCarRequest
    {
        [Required]
        public Guid ScheduleId { get; set; }
        [Required]
        public Guid ClientId { get; set; }
        [Required]
        public DateTime? WithdrawdAt { get; set; }
    }
}