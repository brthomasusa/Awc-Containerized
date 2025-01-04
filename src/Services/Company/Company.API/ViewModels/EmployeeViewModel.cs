namespace Awc.Services.Company.API.ViewModels
{
    public sealed class EmployeeViewModel
    {
        public int BusinessEntityID { get; set; }
        public string? NameStyle { get; set; }
        public string? Title { get; set; }
        public string? EmployeeName { get; set; }
        public string? Suffix { get; set; }
        public string? JobTitle { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? EmailPromotionPreference { get; set; }
        public string? NationalIDNumber { get; set; }
        public string? LoginID { get; set; }
        public string? FullAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Gender { get; set; }
        public DateTime HireDate { get; set; }
        public string? JobClassification { get; set; }
        public int VacationHours { get; set; }
        public int SickLeaveHours { get; set; }
        public decimal PayRate { get; set; }
        public string? PayFrequency { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? ManagerFullName { get; set; }
        public string? Department { get; set; }
        public string? Shift { get; set; }        
        public List<DepartmentHistoryViewModel>? DepartmentHistories { get; set; } = [];
        public List<PayHistoryViewModel>? PayHistories { get; set; } = [];       
    }
}