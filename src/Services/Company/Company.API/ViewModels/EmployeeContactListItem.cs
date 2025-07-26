namespace Awc.Services.Company.API.ViewModels
{
    public class EmployeeContactListItem
    {
        public int EmployeeID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? FullName { get; set; }
        public string? Telephone { get; set; }
        public string? EmailAddress { get; set; }
    }
}