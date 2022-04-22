using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public sealed class OperatorUser : User
    {
        [Required]
        public string Number { get; set; }
    }
}

