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
            int skip,
            int take
        )
        {
            Serilog.ILogger log = Log.ForContext<GetEmployeeViewModelQuery>();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("departmentId", departmentId, DbType.Int32);
                parameters.Add("skip", skip, DbType.Int32);
                parameters.Add("take", take, DbType.Int32);

                string countSql = EmployeeViewModelQuerySql.GetDepartmentMemberViewModelsCount + " WHERE DepartmentID = @departmentId";

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