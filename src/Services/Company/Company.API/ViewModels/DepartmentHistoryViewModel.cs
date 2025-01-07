namespace Awc.Services.Company.API.ViewModels
{
    public sealed class DepartmentHistoryViewModel
    {
        public int BusinessEntityID { get; set; }
        public string? Department { get; set; }
        public string? Shift { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}