using Awc.Services.Company.API.Application.Features.GetDepartmentMembers;

namespace Company.FunctionalTests.Application.Features.GetDepartmentMembers
{
    public class GetDepartmentMembersQueryHandlerTests : TestBase
    {
        private readonly CompanyService _service;

        public GetDepartmentMembersQueryHandlerTests()
        {
            _service = new(_dapperCtx, new NullLogger<CompanyService>());
        }


        [Fact]
        public async Task Handle_GetDepartmentMembersQueryHandler_ValidDepartmentId_ShouldReturn10()
        {
            // Arrange
            int departmentId = 7;
            int skip = 0;
            int take = 10;
            GetDepartmentMembersQuery request = new(departmentId, skip, take);
            GetDepartmentMembersQueryHandler handler = new(_service);

            // Act
            Result<PagedList<DepartmentMemberViewModel>> result = await handler.Handle(request, new CancellationToken());

            // Assert
            Assert.True(result.IsSuccess);
            int members = result.Value.Data.Count;
            Assert.Equal(10, members);
        }
    }
}