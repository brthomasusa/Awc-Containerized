using Awc.Services.Company.API.Application.Features.GetEmployees;
using Awc.Services.Company.API.Infrastructure;
using Microsoft.Extensions.Options;

namespace Company.FunctionalTests.Application.Features.GetEmployees
{
    public class GetEmployeesQueryHandlerTests : TestBase
    {
        private readonly EmployeeService _service;
        private readonly DatabaseRetryService _databaseRetryService;

        public GetEmployeesQueryHandlerTests()
        {
            _service = new(_dapperCtx, new NullLogger<EmployeeService>());

            DatabaseReconnectSettings settings = new() { RetryCount = 5, RetryWaitPeriodInSeconds = 5 };
            IOptions<DatabaseReconnectSettings> databaseReconnectSettingsOptions = Options.Create(settings);
            _databaseRetryService = new DatabaseRetryService(databaseReconnectSettingsOptions);
        }

        [Fact]
        public async Task Handle_GetEmployeesQueryHandler_ValidSearchCriteria_ShouldSucceed()
        {
            // Arrange
            StringSearchCriteria criteria = new("[LastName]", "Du", "[LastName]", 0, 10);
            GetEmployeesQuery request = new(criteria);
            GetEmployeesQueryHandler handler = new(_service, _databaseRetryService);

            // Act
            Result<PagedList<EmployeeListItemViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(3, employees);
        }

        [Fact]
        public async Task Handle_GetEmployeesQueryHandler_InvalidSearchCriteria_ShouldReturnZeroEmployees()
        {
            // Arrange
            StringSearchCriteria criteria = new("[LastName]", "Q", "[LastName]", 0, 10);
            GetEmployeesQuery request = new(criteria);
            GetEmployeesQueryHandler handler = new(_service, _databaseRetryService);

            // Act
            Result<PagedList<EmployeeListItemViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(0, employees);
        }

        [Fact]
        public async Task Handle_GetEmployeesQueryHandler_EmptySearchCriteria_ShouldReturnSevenEmployees()
        {
            // Arrange
            StringSearchCriteria criteria = new("[LastName]", string.Empty, "[LastName]", 0, 10);
            GetEmployeesQuery request = new(criteria);
            GetEmployeesQueryHandler handler = new(_service, _databaseRetryService);

            // Act
            Result<PagedList<EmployeeListItemViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(10, employees);
        }
    }
}