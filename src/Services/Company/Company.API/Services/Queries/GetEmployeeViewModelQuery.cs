using Dapper;
using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Services.Queries
{
    public class GetEmployeeViewModelQuery
    {
        public async static Task<Result<EmployeeDetailViewModel>> DoQuery(DapperContext context, int employeeId)
        {
            Serilog.ILogger log = Log.ForContext<GetEmployeeViewModelQuery>();

            try
            {

                var parameters = new DynamicParameters();
                parameters.Add("EmployeeID", employeeId, DbType.Int32);

                using var conn = context.CreateConnection();
                var model = await conn.QueryFirstOrDefaultAsync<EmployeeDetailViewModel>("HumanResources.spGetEmployeeDetails", 
                                                                                   parameters, 
                                                                                   commandType: CommandType.StoredProcedure);

                if (model is null)
                {
                    string errMsg = $"Unable to retrieve employee details for employee with ID: {employeeId}.";
                    log.Warning(errMsg, employeeId);

                    return Result<EmployeeDetailViewModel>.Failure<EmployeeDetailViewModel>(
                        new Error("GetEmployeeViewModelQuery.DoQuery", errMsg)
                    );
                }

                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));
                return Result<EmployeeDetailViewModel>.Failure<EmployeeDetailViewModel>(
                    new Error("GetEmployeeViewModelQuery.DoQuery", Helpers.GetInnerExceptionMessage(ex))
                );                
            }
        }        
    }
}