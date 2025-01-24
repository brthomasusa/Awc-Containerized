using Dapper;
using System.Text;

namespace Awc.Services.Company.API.Services.Queries
{
    public sealed class GetDepartmentMemberViewModelsQuery
    {
        public async static Task<Result<PagedList<DepartmentMemberViewModel>>> DoQuery
        (
            DapperContext context, 
            int departmentId,
            string lastName,
            int skip,
            int take
        )
        {
            Serilog.ILogger log = Log.ForContext<GetEmployeeViewModelQuery>();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("departmentId", departmentId, DbType.Int32);
                parameters.Add("lastName", lastName, DbType.String);
                parameters.Add("skip", skip, DbType.Int32);
                parameters.Add("take", take, DbType.Int32);

                string countSql = !string.IsNullOrEmpty(lastName) ?                                 
                    $"{EmployeeViewModelQuerySql.GetDepartmentMemberViewModelsCount} WHERE DepartmentID = @departmentId AND LastName LIKE CONCAT(@lastName,'%')" : 
                    $"{EmployeeViewModelQuerySql.GetDepartmentMemberViewModelsCount} WHERE DepartmentID = @departmentId";

                using var connection = context.CreateConnection();
                var items = await connection.QueryAsync<DepartmentMemberViewModel>("HumanResources.spGetEmployeeListIemsByDepartment", 
                                                                                   parameters, 
                                                                                   commandType: CommandType.StoredProcedure);

                int count = connection.ExecuteScalar<int>(countSql, parameters);

                MetaData metaData = new(skip, take, count);
                PagedList<DepartmentMemberViewModel> pagedList = new(metaData, items.ToList());

                return pagedList;                
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));
                return Result<PagedList<DepartmentMemberViewModel>>.Failure<PagedList<DepartmentMemberViewModel>>(
                    new Error("GetDepartmentViewModelsQuery.DoQuery", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }    
    }
}