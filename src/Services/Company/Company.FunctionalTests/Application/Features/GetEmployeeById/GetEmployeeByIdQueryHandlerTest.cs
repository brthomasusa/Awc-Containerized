using Awc.Services.Company.API.Application.Features.GetEmployeeById;
using Moq;

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
            var mockCacheService = new Mock<ICacheService>();

            mockCacheService.Setup(x => x.GetData<EmployeeDetailViewModel>(
                    It.IsAny<string>()
                )).Returns(GetEmployeeDetailViewModel());

            mockCacheService.Setup(x => x.SetData<EmployeeDetailViewModel>(
                    It.IsAny<string>(),
                    new EmployeeDetailViewModel(),
                    It.IsAny<DateTimeOffset>()
                )).Returns(true);

            GetEmployeeByIdQuery request = new(EmployeeId: 16);
            GetEmployeeByIdQueryHandler handler = new(_service, mockCacheService.Object);

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

            mockCacheService.Setup(x => x.GetData<EmployeeDetailViewModel>(
                    It.IsAny<string>()
                )).Returns(model!);

            mockCacheService.Setup(x => x.SetData<EmployeeDetailViewModel>(
                    It.IsAny<string>(),
                    new EmployeeDetailViewModel(),
                    It.IsAny<DateTimeOffset>()
                )).Returns(true);

            GetEmployeeByIdQuery request = new(EmployeeId: -16);
            GetEmployeeByIdQueryHandler handler = new(_service, mockCacheService.Object);

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