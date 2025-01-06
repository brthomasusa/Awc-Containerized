using Awc.Services.Company.API.Application.Abstractions.Messaging;
using Awc.Services.Company.API.ViewModels;
using Awc.Services.Company.API.Services;

namespace Awc.Services.Company.API.Application.Features.GetEmployeeById
{
    public sealed class GetEmployeeByIdQueryHandler(IEmployeeService service) : IQueryHandler<GetEmployeeByIdQuery, EmployeeDetailViewModel>
    {
        private readonly IEmployeeService _service = service;

        public async Task<Result<EmployeeDetailViewModel>> Handle
        (
            GetEmployeeByIdQuery request,
            CancellationToken cancellationToken
        )
        {
            Result<EmployeeDetailViewModel> result = await _service.GetEmployeeViewModelWithChildren(request.EmployeeId);

            if (result.IsFailure)
            {
                return Result<EmployeeDetailViewModel>.Failure<EmployeeDetailViewModel>(
                    new Error("GetEmployeeByIdQueryHandler.Handle", result.Error.Message)
                );
            }

            return result.Value;            
        }
    }
}