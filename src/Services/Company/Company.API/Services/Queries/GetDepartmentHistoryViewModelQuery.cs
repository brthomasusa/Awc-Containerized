using Dapper;
using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Services.Queries
{
    public class GetDepartmentHistoryViewModelQuery
    {
        public async static Task<Result<List<DepartmentHistoryViewModel>>> DoQuery(DapperContext context, int employeeId)
        {
            Serilog.ILogger log = Log.ForContext<GetEmployeeViewModelQuery>();

            try
            {
                string sql = EmployeeViewModelQuerySql.GetDepartmentHistoryViewModel;
                var parameters = new DynamicParameters();
                parameters.Add("ID", employeeId, DbType.Int32);

                using var conn = context.CreateConnection();
                var model = await conn.QueryAsync<DepartmentHistoryViewModel>(sql, parameters);

                if (model is null)
                {
                    string errMsg = $"Unable to retrieve department histories for employee with ID: {employeeId}.";
                    log.Warning(errMsg, employeeId);

                    return Result<List<DepartmentHistoryViewModel>>.Failure<List<DepartmentHistoryViewModel>>(
                        new Error("GetDepartmentHistoryViewModelQuery.DoQuery", errMsg)
                    );
                }

                return model.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));
                return Result<List<DepartmentHistoryViewModel>>.Failure<List<DepartmentHistoryViewModel>>(
                    new Error("GetDepartmentHistoryViewModelQuery.DoQuery", Helpers.GetInnerExceptionMessage(ex))
                );                
            }
        }        
    }
}