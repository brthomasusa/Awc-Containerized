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
            WHERE BusinessEntityID = (@ID)
            ";  

        public const string GetPayHistoryViewModel =
            @"SELECT
                BusinessEntityID
                ,RateChangeDate
                ,PayRate
            FROM HumanResources.vPayHistoryViewModel
            WHERE BusinessEntityID = (@ID)
            ";                  
    }
}