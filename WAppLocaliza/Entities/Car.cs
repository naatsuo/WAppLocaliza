namespace WAppLocaliza.Entities
{
    public enum FuelType
    {
        Gasoline,
        Alcohol,
        Diesel
    }

    public enum CategoryType
    {
        Basic,
        Complete,
        Deluxe
    }

    public class Car
    {
        public Guid Id { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Price { get; set; }
        public string PriceHour { get; set; }
        public FuelType Fuel { get; set; }
        public string TrunkLimit { get; set; }
        public CategoryType Category { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}

