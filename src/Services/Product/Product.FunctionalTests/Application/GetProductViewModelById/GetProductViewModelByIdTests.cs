using Awc.Services.Product.Product.API.Application.Features.Products.GetProductViewModelById;

namespace Product.FunctionalTests.Application.GetProductViewModelById
{
    public class GetProductViewModelByIdTests : TestBase
    {
        private readonly DatabaseRetryService _databaseRetryService;
        private readonly ILogger<GetPrductDetailViewModelByIdQueryHandler> _logger;

        public GetProductViewModelByIdTests()
        {
            DatabaseReconnectSettings settings = new() { RetryCount = 5, RetryWaitPeriodInSeconds = 5 };
            IOptions<DatabaseReconnectSettings> databaseReconnectSettingsOptions = Options.Create(settings);
            _databaseRetryService = new DatabaseRetryService(databaseReconnectSettingsOptions);
            _logger = new NullLogger<GetPrductDetailViewModelByIdQueryHandler>();
        }
        [Fact]
        public async Task Handle_GetPrductDetailViewModelByIdQueryHandler_ValidIdInCache_ShouldSucceed()
        {
            // Arrange
            int productId = 771;
            var mockCacheService = new Mock<ICacheService>();

            mockCacheService.Setup(x => x.GetCacheValueAsync<ProductDetailViewModel>(
                    It.IsIn<string>($"product:{productId}")
                )).ReturnsAsync(new ProductDetailViewModel() { ProductID = productId });

            mockCacheService.Setup(x => x.SetCacheValueAsync<ProductDetailViewModel>(
                    It.IsIn<string>($"product:{productId}"),
                    new ProductDetailViewModel() { ProductID = productId },
                    It.IsAny<TimeSpan>()
                ));


            GetProductViewModelByIdQuery request = new(ProductId: productId);
            GetPrductDetailViewModelByIdQueryHandler handler = new(mockCacheService.Object, _logger, _databaseRetryService, _dapperCtx);

            // Act
            Result<ProductDetailViewModel> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_GetPrductDetailViewModelByIdQueryHandler_ValidIdNotInCache_ShouldSucceed()
        {
            // Arrange
            int productId = 1;
            ProductDetailViewModel? model = null;
            var mockCacheService = new Mock<ICacheService>();

            mockCacheService.Setup(x => x.GetCacheValueAsync<ProductDetailViewModel>(
                    It.IsIn<string>($"product:{productId}")
                )).ReturnsAsync(model!);

            mockCacheService.Setup(x => x.SetCacheValueAsync<ProductDetailViewModel>(
                    It.IsIn<string>($"product:{productId}"),
                    new ProductDetailViewModel() { ProductID = productId },
                    It.IsAny<TimeSpan>()
                ));

            GetProductViewModelByIdQuery request = new(ProductId: productId);
            GetPrductDetailViewModelByIdQueryHandler handler = new(mockCacheService.Object, _logger, _databaseRetryService, _dapperCtx);

            // Act
            Result<ProductDetailViewModel> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_GetPrductDetailViewModelByIdQueryHandler_InValidProductId_ShouldFail()
        {
            // Arrange
            int productId = -1;
            ProductDetailViewModel? model = null;
            var mockCacheService = new Mock<ICacheService>();

            mockCacheService.Setup(x => x.GetCacheValueAsync<ProductDetailViewModel>(
                    It.IsIn<string>($"product:{productId}")
                )).ReturnsAsync(model!);

            GetProductViewModelByIdQuery request = new(ProductId: productId);
            GetPrductDetailViewModelByIdQueryHandler handler = new(mockCacheService.Object, _logger, _databaseRetryService, _dapperCtx);

            // Act
            Result<ProductDetailViewModel> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsFailure);
        }


    }
}