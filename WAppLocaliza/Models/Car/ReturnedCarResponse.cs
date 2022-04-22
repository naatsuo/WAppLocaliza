namespace WAppLocaliza.Models
{
    public class ReturnedCarResponse
    {
        public Guid ScheduleId { get; set; }
        public Guid HistoryId { get; set; }
        public float Price { get; set; }
        public float PricePenalty { get; set; }
        public float PriceTotal { get; set; }
        public double HourTotal { get; set; }
       
    }
}

