using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class CreateModelRequest
    {
        [Required]
        public Guid BrandId { get; set; }
        [Required]
        public string Description { get; set; }
    }
}