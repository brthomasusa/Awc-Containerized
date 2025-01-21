using Awc.Services.Company.API.Application.Features.GetDepartmentMembers;

namespace Company.FunctionalTests.Services
{
    public class CompanyServiceTests : TestBase
    {
        [Fact]
        public async Task GetDepartmentMemberViewModels_CompanyService_ValidId_ShouldSucceed()
        {
            // Arrange
            int departmentId = 7;
            string lastName = "Du";
            int skip = 0;
            int take = 10;
            
            CompanyService service = new(_dapperCtx, new NullLogger<CompanyService>());

            // Act
            Result<PagedList<DepartmentMemberViewModel>> result = await service.GetDepartmentMemberViewModels(departmentId, lastName, skip, take);

            // Assert
            Assert.True(result.IsSuccess);
            int members = result.Value.Data.Count;
            Assert.Equal(2, members);            
        }



    }
}