using System.Text.Json.Serialization;

namespace WAppLocaliza.Entities
{
    public class Operator
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastAccessAt { get; set; }
        public string[] Roles { get; set; }
    }

    public class Car
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }
        public string PriceHour { get; set; }
        public string Fuel { get; set; } //(gasolina, álcool, diesel)
        public string TrunkLimit { get; set; }
        public string Category { get; set; }  //(básico, completo, luxo)
        public DateTime CreatedAt { get; set; }
    }
}

