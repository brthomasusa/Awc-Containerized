namespace WebUI.Models.CompanyApi
{
    public class ShiftViewModel
    {
        public byte ShiftID { get; set; }
        public string? Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public string? StartTimeToString => StartTime.ToString("H:mm");
        public TimeSpan EndTime { get; set; }
        public string? EndTimeToString => EndTime.ToString("h:mm tt");
    }
}