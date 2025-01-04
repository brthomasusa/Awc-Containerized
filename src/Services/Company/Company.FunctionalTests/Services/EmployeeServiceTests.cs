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
            Result<EmployeeViewModel> result = await service.GetEmployeeViewModelWithChildren(employeeId);

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
            Result<EmployeeViewModel> result = await service.GetEmployeeViewModelWithChildren(employeeId);

            // Assert
            Assert.True(result.IsFailure);
        }        
    }
}