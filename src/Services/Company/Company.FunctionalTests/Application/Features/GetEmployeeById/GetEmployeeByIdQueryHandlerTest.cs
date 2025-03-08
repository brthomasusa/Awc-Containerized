using Awc.Services.Company.API.Application.Features.GetEmployeeById;
using Awc.Services.Company.API.Infrastructure;
using Microsoft.Extensions.Options;
using Moq;

namespace Company.FunctionalTests.Application.Features.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandlerTest : TestBase
    {
        private readonly EmployeeService _service;
        private readonly DatabaseRetryService _databaseRetryService;

        public GetEmployeeByIdQueryHandlerTest()
        {
            _service = new(_dapperCtx, new NullLogger<EmployeeService>());

            DatabaseReconnectSettings settings = new() { RetryCount = 5, RetryWaitPeriodInSeconds = 5 };
            IOptions<DatabaseReconnectSettings> databaseReconnectSettingsOptions = Options.Create(settings);
            _databaseRetryService = new DatabaseRetryService(databaseReconnectSettingsOptions);
        }

        [Fact]
        public async Task Handle_GetEmployeeByIdQueryHandler_ValidId_ShouldSucceed()
        {
            // Arrange
            var mockCacheService = new Mock<ICacheService>();

            mockCacheService.Setup(x => x.GetCacheValueAsync<EmployeeDetailViewModel>(
                    It.IsAny<string>()
                )).ReturnsAsync(GetEmployeeDetailViewModel());

            mockCacheService.Setup(x => x.SetCacheValueAsync<EmployeeDetailViewModel>(
                    It.IsAny<string>(),
                    new EmployeeDetailViewModel(),
                    It.IsAny<TimeSpan>()
                ));

            GetEmployeeByIdQuery request = new(EmployeeId: 16);
            GetEmployeeByIdQueryHandler handler = new(_service, mockCacheService.Object, _databaseRetryService);

            // Act
            Result<EmployeeDetailViewModel> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            // Assert.NotEmpty(result.Value.DepartmentHistories!);
            // Assert.NotEmpty(result.Value.PayHistories!);
        }

        [Fact]
        public async Task Handle_GetEmployeeByIdQueryHandler_InvalidId_ShouldFail()
        {
            // Arrange
            EmployeeDetailViewModel? model = null;

            var mockCacheService = new Mock<ICacheService>();

            mockCacheService.Setup(x => x.GetCacheValueAsync<EmployeeDetailViewModel>(
                    It.IsAny<string>()
                )).ReturnsAsync(model!);

            mockCacheService.Setup(x => x.SetCacheValueAsync<EmployeeDetailViewModel>(
                    It.IsAny<string>(),
                    new EmployeeDetailViewModel(),
                    It.IsAny<TimeSpan>()
                ));

            GetEmployeeByIdQuery request = new(EmployeeId: -16);
            GetEmployeeByIdQueryHandler handler = new(_service, mockCacheService.Object, _databaseRetryService);

            // Act
            Result<EmployeeDetailViewModel> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsFailure);
        }

        private static EmployeeDetailViewModel GetEmployeeDetailViewModel()
        {
            return new EmployeeDetailViewModel
            {
                BusinessEntityID = 16,
            };
        }
    }
}