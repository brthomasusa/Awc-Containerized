using Awc.Services.Product.Product.API.Application.Features.Products.GetProductListItemsByName;
using Awc.Services.Product.Product.API.Application.Abstractions;

namespace Product.FunctionalTests.Application.GetProductListItemViewModelsByName
{
    public class GetProductListItemsByNameQueryHandlerTests : TestBase
    {
        private readonly DatabaseRetryService _databaseRetryService;
        private readonly ILogger<GetProductListItemsByNameQueryHandler> _logger;

        public GetProductListItemsByNameQueryHandlerTests()
        {
            DatabaseReconnectSettings settings = new() { RetryCount = 5, RetryWaitPeriodInSeconds = 5 };
            IOptions<DatabaseReconnectSettings> databaseReconnectSettingsOptions = Options.Create(settings);
            _databaseRetryService = new DatabaseRetryService(databaseReconnectSettingsOptions);
            _logger = new NullLogger<GetProductListItemsByNameQueryHandler>();
        }

        [Fact]
        public async Task Handle_GetProductListItemsByNameQueryHandler_NotNullCriteria_ShouldSucceed()
        {
            // Arrange
            StringSearchCriteria criteria = new("[Name]", "tube", string.Empty, 0, 10);
            GetProductListItemsByNameQuery request = new(criteria);
            GetProductListItemsByNameQueryHandler handler = new(_logger, _databaseRetryService, _dapperCtx);

            // Act
            Result<PagedList<ProductListItemViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int products = result.Value.Data.Count;
            int totalRecords = result.Value.MetaData!.TotalRecords;

            Assert.Equal(8, products);
            Assert.Equal(8, totalRecords);
        }

        [Fact]
        public async Task Handle_GetProductListItemsByNameQueryHandler_NullCriteria_ShouldSucceed()
        {
            // Arrange
            StringSearchCriteria criteria = new("[Name]", null, string.Empty, 0, 30);
            GetProductListItemsByNameQuery request = new(criteria);
            GetProductListItemsByNameQueryHandler handler = new(_logger, _databaseRetryService, _dapperCtx);

            // Act
            Result<PagedList<ProductListItemViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int products = result.Value.Data.Count;
            int totalRecords = result.Value.MetaData!.TotalRecords;

            Assert.Equal(30, products);
            Assert.Equal(504, totalRecords);
        }

        [Fact]
        public async Task Handle_GetProductListItemsByNameQueryHandler_NonExistingCriteria_ShouldSucceed()
        {
            // Arrange
            StringSearchCriteria criteria = new("[Name]", ",.;", string.Empty, 0, 10);
            GetProductListItemsByNameQuery request = new(criteria);
            GetProductListItemsByNameQueryHandler handler = new(_logger, _databaseRetryService, _dapperCtx);

            // Act
            Result<PagedList<ProductListItemViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int products = result.Value.Data.Count;
            int totalRecords = result.Value.MetaData!.TotalRecords;

            Assert.Equal(0, products);
            Assert.Equal(0, totalRecords);
        }
    }
}