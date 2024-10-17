namespace Awc.Services.Company.API.ViewModels
{
    public class ShiftViewModel
    {
        public byte ShiftID { get; set; }
        public string? Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }        
    }
}