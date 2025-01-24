using Dapper;
using System.Text;
using Awc.Services.Company.API.Application.Features.GetEmployees;

namespace Awc.Services.Company.API.Services.Queries
{
    public sealed class GetEmployeeListItemsQuery
    {
        public async static Task<Result<PagedList<EmployeeListItemViewModel>>> DoQuery(DapperContext context, StringSearchCriteria criteria)
        {
            Serilog.ILogger log = Log.ForContext<GetEmployeeViewModelQuery>();

            try
            {
                var parameters = new DynamicParameters();

                StringBuilder sb = new();
                sb.Append(EmployeeViewModelQuerySql.GetEmployeeListItems);

                if (!string.IsNullOrEmpty(criteria.SearchField) && !string.IsNullOrEmpty(criteria.SearchCriteria))
                {
                    sb.Append(" WHERE ")
                      .Append(criteria.SearchField)
                      .Append(" LIKE CONCAT(@CRITERIA,'%') ");
                }

                if (!string.IsNullOrEmpty(criteria.OrderBy))
                    sb.Append(" ORDER BY ").Append(criteria.OrderBy);
                else
                    sb.Append(" ORDER BY LastName, FirstName, MiddleName");

                sb.Append(" OFFSET @SKIP ROWS FETCH NEXT @TAKE ROWS ONLY");

                parameters.Add("CRITERIA", criteria.SearchCriteria, DbType.String);
                parameters.Add("SKIP", criteria.Skip, DbType.Int32);
                parameters.Add("TAKE", criteria.Take, DbType.Int32);

                string countSql = !string.IsNullOrEmpty(criteria.SearchCriteria) &&
                                  !string.IsNullOrEmpty(criteria.SearchField) ?
                    $"{EmployeeViewModelQuerySql.GetEmployeeListItemsCount} WHERE {criteria.SearchField} LIKE CONCAT(@Criteria,'%')" :
                    EmployeeViewModelQuerySql.GetEmployeeListItemsCount;

                using var connection = context.CreateConnection();

                var items = (await connection.QueryAsync<EmployeeListItemViewModel>(sb.ToString(), parameters)).ToList();
                int count = connection.ExecuteScalar<int>(countSql, parameters);

                MetaData metaData = new(criteria.Skip, criteria.Take, count);
                PagedList<EmployeeListItemViewModel> pagedList = new(metaData, items);

                return pagedList;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));
                return Result<PagedList<EmployeeListItemViewModel>>.Failure<PagedList<EmployeeListItemViewModel>>(
                    new Error("GetEmployeeListItemsQuery.DoQuery", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}