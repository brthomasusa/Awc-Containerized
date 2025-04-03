

namespace Awc.Services.Product.Product.API.Application.Features.Products.GetProductListItemsByName
{
    public sealed class GetProductListItemsByNameQueryHandler
    (
        ILogger<GetProductListItemsByNameQueryHandler> logger,
        IDatabaseRetryService databaseRetryService,
        DapperContext context
    ) : IQueryHandler<GetProductListItemsByNameQuery, PagedList<ProductListItemViewModel>>
    {
        private readonly ILogger<GetProductListItemsByNameQueryHandler> _logger = logger;
        private readonly IDatabaseRetryService _databaseRetryService = databaseRetryService;
        private readonly DapperContext _context = context;

        public async Task<Result<PagedList<ProductListItemViewModel>>> Handle
        (
            GetProductListItemsByNameQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("searchCriteria", query.SearchCriteria.SearchCriteria, DbType.String);
                parameters.Add("skip", query.SearchCriteria.Skip, DbType.Int32);
                parameters.Add("take", query.SearchCriteria.Take, DbType.Int32);

                string countSql = !string.IsNullOrEmpty(query.SearchCriteria.SearchCriteria) ?
                    $"SELECT COUNT(*) from Production.vProductListItemViewModel WHERE [Name] LIKE CONCAT('%',@searchCriteria,'%')" :
                    $"SELECT COUNT(*) from Production.vProductListItemViewModel";

                int count = 0;
                IEnumerable<ProductListItemViewModel>? items = null;

                using var connection = _context.CreateConnection();

                // Retry attempts and timing is configured in appsettings.json
                await _databaseRetryService.ExecuteWithRetryAsync(async () =>
                {
                    items = await connection.QueryAsync<ProductListItemViewModel>("Production.spGetProductListIemsByName",
                                                                                  parameters,
                                                                                  commandType: CommandType.StoredProcedure);

                    count = connection.ExecuteScalar<int>(countSql, parameters);
                });

                MetaData metaData = new(query.SearchCriteria.Skip, query.SearchCriteria.Take, count);
                PagedList<ProductListItemViewModel> pagedList = new(metaData, [.. items!]);

                return pagedList;
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));

                return Result<PagedList<ProductListItemViewModel>>.Failure<PagedList<ProductListItemViewModel>>(
                    new Error("GetProductListItemsByNameQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex)));
            }
        }
    }
}