using Awc.Services.Company.API.Application.Features.GetEmployees;
using Awc.Services.Company.API.ViewModels;
using Awc.Services.Company.API.Services;

namespace Company.FunctionalTests.Services
{
    public class EmployeeServiceTests : TestBase
    {
        [Fact]
        public async Task GetEmployeeViewModelWithChildren_EmployeeService_ValidId_ShouldSucceed()
        {
            // Arrange
            int employeeId = 16;
            EmployeeService service = new(_dapperCtx, new NullLogger<EmployeeService>());

            // Act
            Result<EmployeeDetailViewModel> result = await service.GetEmployeeViewModelWithChildren(employeeId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value.DepartmentHistories!);
            Assert.NotEmpty(result.Value.PayHistories!);
        }

        [Fact]
        public async Task GetEmployeeViewModelWithChildren_EmployeeService_InvalidId_ShouldFail()
        {
            // Arrange
            int employeeId = -16;
            EmployeeService service = new(_dapperCtx, new NullLogger<EmployeeService>());

            // Act
            Result<EmployeeDetailViewModel> result = await service.GetEmployeeViewModelWithChildren(employeeId);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public async Task GetEmployeeListItems_EmployeeService_ValidCriteria_ShouldSucceed()
        {
            // Arrange
            StringSearchCriteria criteria = new("[LastName]", "Du", "[LastName]", 0, 10);
            EmployeeService service = new(_dapperCtx, new NullLogger<EmployeeService>());

            // Act
            Result<PagedList<EmployeeListItemViewModel>> result = await service.GetEmployeeListItems(criteria);


            // Assert
            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(4, employees);
        }

        [Fact]
        public async Task GetEmployeeListItems_EmployeeService_NoSearchCriteria_ShouldSucceed()
        {
            // Arrange
            StringSearchCriteria criteria = new("[LastName]", string.Empty, "[LastName]", 0, 10);
            EmployeeService service = new(_dapperCtx, new NullLogger<EmployeeService>());

            // Act
            Result<PagedList<EmployeeListItemViewModel>> result = await service.GetEmployeeListItems(criteria);


            // Assert
            Assert.True(result.IsSuccess);
            int employees = result.Value.Data.Count;
            Assert.Equal(10, employees);
        }
    }
}