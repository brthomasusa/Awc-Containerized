namespace Awc.Dapr.Services.Company.API.Model.Company
{
    public sealed class Shift
    {
        public byte ShiftID { get; set; }
        public string? Name { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public DateTime ModifiedDate { get; set; }        
    }
}