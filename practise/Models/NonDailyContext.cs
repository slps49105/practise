namespace practise.Models
{
    public class NonDailyContext
    {
        public int Id {  get; set; }
        public string? ItemName { get; set; }
        public string? ItemStatus { get; set; }
        public string? UserName { get; set; }
        public DateTime ResetTime { get; set; }
    }
}
