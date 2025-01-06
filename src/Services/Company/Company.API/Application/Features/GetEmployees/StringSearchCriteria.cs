using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Awc.Services.Company.API.Application.Features.GetEmployees
{
    public sealed record StringSearchCriteria
    (
        string? SearchField,
        string? SearchCriteria,
        string? OrderBy,
        int PageNumber,
        int PageSize,
        int Skip,
        int Take
    );
}