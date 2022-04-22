using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public class CarSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [JsonIgnore]
        [Required]
        public ClientUser Client { get; set; }
        

        [Required]
        public DateTime Start { get; set; }
        [Required]
        public DateTime End { get; set; }
        [Required]
        public float Price { get; set; }

        public DateTime? WithdrawdAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
        public float? RealPrice { get; set; }

        [Column(TypeName = "VARCHAR(MAX)")]
        public string Note { get; set; }

        [JsonIgnore]
        [Required]
        public Car Car { get; set; }
    }
}

