namespace WAppLocaliza.Models
{
    public class SimulateCarResponse
    {
        public Guid CarId { get; set; }
        public float PriceHour { get; set; }
        public float PriceTotal { get; set; }
        public double HourTotal { get; set; }
        public bool Available { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}