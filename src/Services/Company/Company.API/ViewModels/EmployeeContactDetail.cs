namespace Awc.Services.Company.API.ViewModels
{
    public class EmployeeContactDetail
    {
        public int BusinessEntityID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? FullName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public string? Shift { get; set; }
        public string? ManagerName { get; set; }
        public string? Telephone { get; set; }
        public string? EmailAddress { get; set; }
        public string? EmploymentStatus { get; set; }
    }
}