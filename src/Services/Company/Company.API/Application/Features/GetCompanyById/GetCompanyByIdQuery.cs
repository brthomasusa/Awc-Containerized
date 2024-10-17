using Awc.Dapr.Services.Company.API.Application.Abstractions.Messaging;
using Awc.Dapr.Services.Company.API.ViewModels;

namespace Awc.Dapr.Services.Company.API.Application.Features.GetCompanyById
{
    public sealed record GetCompanyByIdQuery(int CompanyId) : IQuery<CompanyViewModel>;
}