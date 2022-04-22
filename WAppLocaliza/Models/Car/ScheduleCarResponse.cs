namespace WAppLocaliza.Models
{
    public class ScheduleCarResponse
    {
        public Guid ScheduleId { get; set; }
        public float PriceHour { get; set; }
        public float PriceTotal { get; set; }
        public double HourTotal { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}

