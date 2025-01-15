using Awc.Services.Company.API.Application.Features.GetEmployees;


namespace Company.FunctionalTests.Application.Features.GetEmployees
{
    public class GetEmployeesQueryHandlerTests : TestBase
    {
        private readonly EmployeeService _service;

        public GetEmployeesQueryHandlerTests()
        {
            _service = new(_dapperCtx, new NullLogger<EmployeeService>());
        }

        [Fact]
        public async Task Handle_GetEmployeesQueryHandler_ValidSearchCriteria_ShouldSucceed()
        {
            // Arrange
            StringSearchCriteria criteria = new("[LastName]", "Du", "[LastName]", 0, 10);
            GetEmployeesQuery request = new(criteria);
            GetEmployeesQueryHandler handler = new(_service);

            // Act
            Result<PagedList<EmployeeListItemViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(4, employees);
        }

        [Fact]
        public async Task Handle_GetEmployeesQueryHandler_InvalidSearchCriteria_ShouldReturnZeroEmployees()
        {
            // Arrange
            StringSearchCriteria criteria = new("[LastName]", "Q", "[LastName]", 0, 10);
            GetEmployeesQuery request = new(criteria);
            GetEmployeesQueryHandler handler = new(_service);

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
            GetEmployeesQueryHandler handler = new(_service);

            // Act
            Result<PagedList<EmployeeListItemViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(10, employees);
        }
    }
}