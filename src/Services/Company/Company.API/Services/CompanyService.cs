namespace Awc.Services.Company.API.Services
{
    public sealed class CompanyService(DapperContext dapperContext, ILogger<CompanyService> logger) : ICompanyService
    {
        private readonly DapperContext _dapperContext = dapperContext;
        private readonly ILogger<CompanyService> _logger = logger;

        public async Task<Result<CompanyViewModel>> GetCompanyViewModel(int id)
        {
            Result<List<DepartmentViewModel>> getDepartments = await GetDepartments();
            if (getDepartments.IsFailure)
            {
                return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                    new Error("CompanyService.GetCompanyById", getDepartments.Error.Message)
                );
            }

            Result<List<ShiftViewModel>> getShifts = await GetShifts();
            if (getShifts.IsFailure)
            {
                return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                    new Error("CompanyService.GetCompanyById", getShifts.Error.Message)
                );
            }

            Result<CompanyViewModel> getCompany = await GetCompanyById(id);
            if (getCompany.IsFailure)
            {
                return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                    new Error("CompanyService.GetCompanyById", getCompany.Error.Message)
                );
            }

            CompanyViewModel company = getCompany.Value;
            company.Departments = getDepartments.Value;
            company.Shifts = getShifts.Value;

            return company;
        }

        public async Task<Result<PagedList<DepartmentMemberViewModel>>> GetDepartmentMemberViewModels(int departmentId, string lastName, int skip, int take)
        {
            Result<PagedList<DepartmentMemberViewModel>> result =
                await GetDepartmentMemberViewModelsQuery.DoQuery(_dapperContext, departmentId, lastName, skip, take);

            if (result.IsFailure)
            {
                return Result<PagedList<DepartmentMemberViewModel>>.Failure<PagedList<DepartmentMemberViewModel>>(
                    new Error("EmployeeService.GetEmployeeListItems", result.Error.Message)
                );
            }

            return result;            
        }

        private async Task<Result<CompanyViewModel>> GetCompanyById(int id)
        {
            const string sql =
                @"SELECT 
                    CompanyID, CompanyName, LegalName, EIN, WebsiteUrl,
                    MailAddressLine1, MailAddressLine2, MailCity, province.StateProvinceCode AS MailState, MailPostalCode,
                    DeliveryAddressLine1, DeliveryAddressLine2, DeliveryPostalCode, province.StateProvinceCode AS DeliveryState, DeliveryPostalCode,
                    Telephone, Fax  
                FROM Person.Company company
                INNER JOIN Person.StateProvince province ON province.StateProvinceID = company.MailStateProvinceID
                WHERE CompanyID = @ID";

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", id, DbType.Int32);

                using var connection = _dapperContext.CreateConnection();
                CompanyViewModel? detail = await connection.QueryFirstOrDefaultAsync<CompanyViewModel>(sql, parameters);

                if (detail is null)
                {
                    string errMsg = $"Unable to retrieve company details for company with ID: {id}.";
                    _logger.LogWarning("{Message}", errMsg);

                    return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                        new Error("CompanyService.GetCompanyById", errMsg)
                    );
                }

                return detail;
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);
                return Result<CompanyViewModel>.Failure<CompanyViewModel>(
                    new Error("CompanyService.GetCompanyById", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }

        private async Task<Result<List<DepartmentViewModel>>> GetDepartments()
        {
            const string sql =
                @"SELECT 
                    DepartmentID, Name, GroupName 
                FROM HumanResources.Department
                ORDER BY Name";

            try
            {
                using var connection = _dapperContext.CreateConnection();
                var departments = await connection.QueryAsync<DepartmentViewModel>(sql);

                return departments.ToList();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);
                return Result<List<DepartmentViewModel>>.Failure<List<DepartmentViewModel>>(
                    new Error("CompanyService.GetDepartments", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }

        private async Task<Result<List<ShiftViewModel>>> GetShifts()
        {
            const string sql =
                @"SELECT 
                    ShiftID, Name, StartTime, EndTime 
                FROM HumanResources.Shift
                ORDER BY Name";

            try
            {
                using var connection = _dapperContext.CreateConnection();
                var shifts = await connection.QueryAsync<ShiftViewModel>(sql);

                return shifts.ToList();
            }
            catch (Exception ex)
            {
                string errMsg = Helpers.GetInnerExceptionMessage(ex);
                _logger.LogError(ex, "{Message}", errMsg);
                return Result<List<ShiftViewModel>>.Failure<List<ShiftViewModel>>(
                    new Error("CompanyService.GetDepartments", Helpers.GetInnerExceptionMessage(ex))
                );
            }
        }
    }
}