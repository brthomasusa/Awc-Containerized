namespace WebUI.Models.CompanyApi
{
    public sealed class DepartmentMemberViewModel
    {
        public int EmployeeID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? JobTitle { get; set; }
        public string? Department { get; set; }
        public string? GroupName { get; set; }
        public string? Shift { get; set; }
        public string? PhoneNumber { get; set; }
        public string? EmailAddress { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? FullName { get; set; }
        public string? ManagerName { get; set; }
        public int EmployeesManaged { get; set; }
    }
}