using Dapper;
using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Services.Queries
{
    public class GetPayHistoryViewModelQuery
    {
        public async static Task<Result<List<PayHistoryViewModel>>> DoQuery(DapperContext context, int employeeId)
        {
            Serilog.ILogger log = Log.ForContext<GetEmployeeViewModelQuery>();

            try
            {
                string sql = EmployeeViewModelQuerySql.GetPayHistoryViewModel;
                var parameters = new DynamicParameters();
                parameters.Add("ID", employeeId, DbType.Int32);

                using var conn = context.CreateConnection();
                var model = await conn.QueryAsync<PayHistoryViewModel>(sql, parameters);

                if (model is null)
                {
                    string errMsg = $"Unable to retrieve pay histories for employee with ID: {employeeId}.";
                    log.Warning(errMsg, employeeId);

                    return Result<List<PayHistoryViewModel>>.Failure<List<PayHistoryViewModel>>(
                        new Error("GetPayHistoryViewModelQuery.DoQuery", errMsg)
                    );
                }

                return model.ToList();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));
                return Result<List<PayHistoryViewModel>>.Failure<List<PayHistoryViewModel>>(
                    new Error("GetPayHistoryViewModelQuery.DoQuery", Helpers.GetInnerExceptionMessage(ex))
                );                
            }
        }        
    }
}