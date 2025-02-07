using Awc.Services.Company.API.Application.Features.GetDepartmentMembers;
using Awc.Services.Company.API.Infrastructure;
using Microsoft.Extensions.Options;

namespace Company.FunctionalTests.Application.Features.GetDepartmentMembers
{
    public class GetDepartmentMembersQueryHandlerTests : TestBase
    {
        private readonly CompanyService _service;
        private readonly DatabaseRetryService _databaseRetryService;

        public GetDepartmentMembersQueryHandlerTests()
        {
            _service = new(_dapperCtx, new NullLogger<CompanyService>());

            DatabaseReconnectSettings settings = new() { RetryCount = 5, RetryWaitPeriodInSeconds = 5 };
            IOptions<DatabaseReconnectSettings> databaseReconnectSettingsOptions = Options.Create(settings);
            _databaseRetryService = new DatabaseRetryService(databaseReconnectSettingsOptions);
        }


        [Fact]
        public async Task Handle_GetDepartmentMembersQueryHandler_ValidDepartmentId_ShouldReturn10()
        {
            // Arrange
            int departmentId = 7;
            string lastName = string.Empty;
            int skip = 0;
            int take = 10;
            GetDepartmentMembersQuery request = new(departmentId, lastName, skip, take);
            GetDepartmentMembersQueryHandler handler = new(_service, _databaseRetryService);

            // Act
            Result<PagedList<DepartmentMemberViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int members = result.Value.Data.Count;
            Assert.Equal(10, members);
        }
    }
}