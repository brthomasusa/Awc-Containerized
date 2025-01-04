using Awc.Services.Company.API.Application.Abstractions.Messaging;
using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Application.Features.GetEmployeeById
{
    public sealed record GetEmployeeByIdQuery(int EmployeeId) : IQuery<EmployeeViewModel>;
}