using Awc.Services.Company.API.Application.Abstractions.Messaging;
using Awc.Services.Company.API.ViewModels;

namespace Awc.Services.Company.API.Application.Features.GetCompanyById
{
    public sealed record GetCompanyByIdQuery(int CompanyId) : IQuery<CompanyViewModel>;
}