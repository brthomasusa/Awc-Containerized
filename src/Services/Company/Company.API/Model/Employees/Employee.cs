namespace Awc.Services.Company.API.Model.Employees
{
    public class Employee
    {
        public int BusinessEntityID { get; set; }
        public int ManagerID { get; set; }
        public string? NationalIDNumber { get; set; }
        public string? LoginID { get; set; }
        public string? JobTitle { get; set; }
        public DateTime BirthDate { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Gender { get; set; }
        public DateTime HireDate { get; set; }
        public bool SalariedFlag { get; set; }
        public int VacationHours { get; set; }
        public int SickLeaveHours { get; set; }
        public bool CurrentFlag { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual List<EmployeeDepartmentHistory> DepartmentHistories { get; set; } = [];
        public virtual List<EmployeePayHistory> PayHistories { get; set; } = [];        
    }
}