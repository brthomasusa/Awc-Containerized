using Awc.Services.Company.API.Application.Features.GetEmployeeById;

namespace Company.FunctionalTests.Application.Features.GetEmployeeById
{
    public class GetEmployeeByIdQueryHandlerTest : TestBase
    {
        private readonly EmployeeService _service;

        public GetEmployeeByIdQueryHandlerTest()
        {
            _service = new(_dapperCtx, new NullLogger<EmployeeService>());
        }

        [Fact]
        public async Task Handle_GetEmployeeByIdQueryHandler_ValidId_ShouldSucceed()
        {
            // Arrange
            GetEmployeeByIdQuery request = new(EmployeeId: 16);
            GetEmployeeByIdQueryHandler handler = new(_service);

            // Act
            Result<EmployeeDetailViewModel> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotEmpty(result.Value.DepartmentHistories!);
            Assert.NotEmpty(result.Value.PayHistories!);
        }

        [Fact]
        public async Task Handle_GetEmployeeByIdQueryHandler_InvalidId_ShouldFail()
        {
            // Arrange
            GetEmployeeByIdQuery request = new(EmployeeId: -16);
            GetEmployeeByIdQueryHandler handler = new(_service);

            // Act
            Result<EmployeeDetailViewModel> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsFailure);
        }        
    }
}