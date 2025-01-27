using Awc.Services.Company.API.Application.Abstractions.Messaging;
using Awc.Services.Company.API.ViewModels;
using Awc.Services.Company.API.Services;

namespace Awc.Services.Company.API.Application.Features.GetEmployeeById
{
    public sealed class GetEmployeeByIdQueryHandler(IEmployeeService service, ICacheService cacheService)
        : IQueryHandler<GetEmployeeByIdQuery, EmployeeDetailViewModel>
    {
        private readonly IEmployeeService _service = service;
        private readonly ICacheService _cacheService = cacheService;

        public async Task<Result<EmployeeDetailViewModel>> Handle
        (
            GetEmployeeByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            var cacheData = _cacheService.GetData<EmployeeDetailViewModel>($"employee{request.EmployeeId}");

            if (cacheData is not null)
            {
                return cacheData;
            }

            Result<EmployeeDetailViewModel> result = await _service.GetEmployeeViewModelWithChildren(request.EmployeeId);

            if (result.IsFailure)
            {
                return Result<EmployeeDetailViewModel>.Failure<EmployeeDetailViewModel>(
                    new Error("GetEmployeeByIdQueryHandler.Handle", result.Error.Message)
                );
            }

            var expireyTime = DateTimeOffset.Now.AddSeconds(60);
            _cacheService.SetData<EmployeeDetailViewModel>($"employee{result.Value.BusinessEntityID}", result.Value, expireyTime);

            return result.Value;
        }
    }
}