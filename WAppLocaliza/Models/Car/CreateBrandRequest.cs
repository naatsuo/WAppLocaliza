using System.ComponentModel.DataAnnotations;

namespace WAppLocaliza.Models
{
    public class CreateBrandRequest
    {
        [Required]
        public string Name { get; set; }
    }
}