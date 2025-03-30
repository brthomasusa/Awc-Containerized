namespace Awc.Services.Product.Product.API.Application.Features.Products.GetProductViewModelById
{
    public sealed class GetPrductDetailViewModelByIdQueryHandler
    (
        ICacheService cacheService,
        ILogger<GetPrductDetailViewModelByIdQueryHandler> logger,
        IDatabaseRetryService databaseRetryService,
        DapperContext context
    )
    {
        private readonly ICacheService _cacheService = cacheService;
        private readonly ILogger<GetPrductDetailViewModelByIdQueryHandler> _logger = logger;
        private readonly IDatabaseRetryService _databaseRetryService = databaseRetryService;
        private readonly DapperContext _context = context;
        private static readonly TimeSpan _cacheExpiration = TimeSpan.FromDays(1);

        public async Task<Result<ProductDetailViewModel>> Handle
        (
            GetProductViewModelByIdQuery query,
            CancellationToken cancellationToken
        )
        {
            try
            {
                // Check if product is already in the cache
                var cacheKey = $"product:{query.ProductId}";
                var cacheData = await _cacheService.GetCacheValueAsync<ProductDetailViewModel>(cacheKey);

                if (cacheData is not null)
                {
                    return cacheData;
                }

                // Not found in the cache, fetch from the database
                var parameters = new DynamicParameters();
                parameters.Add("ID", query.ProductId, DbType.Int32);

                string sql =
                @"SELECT            
					ProductID, 
					Name, 
					ProductNumber, 
					MakeFlag, 
					FinishedGoodsFlag, 
					Color, 
					SafetyStockLevel, 
					ReorderPoint, 
					StandardCost,
					ListPrice, 
					Size, 
					SizeUnitOfMeasurement, 
					WeightUnitOfMeasurement, 
					Weight, 
					DaysToManufacture, 
					ProductLine, 
					Class,
					Style, 
					ProductSubCategory, 
					ProductModel, 
					SellStartDate, 
					SellEndDate, 
					DiscontinuedDate                       
                FROM Production.vProductDetailViewModel
                WHERE ProductID = @ID";

                ProductDetailViewModel? model = null;
                using var conn = _context.CreateConnection();

                // Retry, maybe database is unavailable due to transient issues
                // Retry attempts and timing is configured in appsettings.json
                await _databaseRetryService.ExecuteWithRetryAsync(async () =>
                {
                    model = await conn.QueryFirstOrDefaultAsync<ProductDetailViewModel>(sql, parameters);
                });

                if (model is null)
                {
                    string errMsg = $"Unable to retrieve details for product with ID: {query.ProductId}.";
                    _logger.LogWarning("Warning: {@MSG}", errMsg);

                    return Result<ProductDetailViewModel>.Failure<ProductDetailViewModel>(
                        new Error("GetPrductDetailViewModelByIdQueryHandler.Handle", errMsg)
                    );
                }

                // Add to cache. For now, 10 minutes is arbitrary!
                var expireyTime = DateTimeOffset.Now.AddMinutes(10);
                await _cacheService.SetCacheValueAsync<ProductDetailViewModel>(cacheKey, model, _cacheExpiration);

                return model;
            }
            catch (Exception ex)
            {
                {
                    _logger.LogError("An error occurred: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));

                    return Result<ProductDetailViewModel>.Failure<ProductDetailViewModel>(
                        new Error("GetPrductDetailViewModelByIdQueryHandler.Handle", Helpers.GetInnerExceptionMessage(ex)));
                }
            }
        }
    }
}