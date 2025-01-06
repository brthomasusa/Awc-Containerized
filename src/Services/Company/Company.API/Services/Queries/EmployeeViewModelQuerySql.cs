namespace Awc.Services.Company.API.Services.Queries
{
    public static class EmployeeViewModelQuerySql
    {
        public const string GetDepartmentHistoryViewModel =
            @"SELECT
                BusinessEntityID
                ,Shift
                ,Department
                ,StartDate
                ,EndDate
            FROM HumanResources.vDepartmentHistoryViewModel
            WHERE BusinessEntityID = (@ID)";  

        public const string GetPayHistoryViewModel =
            @"SELECT
                BusinessEntityID
                ,RateChangeDate
                ,PayRate
            FROM HumanResources.vPayHistoryViewModel
            WHERE BusinessEntityID = (@ID)"; 

        public const string GetEmployeeListItems =
        @"SELECT 
            BusinessEntityID           
            ,LastName
            ,FirstName
            ,MiddleName
            ,JobTitle
            ,Department 
            ,JobTitle
            ,Department
            ,Shift
            ,ManagerName 
            ,EmploymentStatus                      
        FROM HumanResources.vEmployeeListItems"; 

        public const string GetEmployeeListItemsCount =
        @"SELECT 
            COUNT(*)               
        FROM HumanResources.vEmployeeListItems";                                    
    }
}