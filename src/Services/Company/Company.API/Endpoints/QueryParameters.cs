using Microsoft.AspNetCore.Http;

namespace Awc.Services.Company.API.Endpoints
{
    public sealed class QueryParameters
    {
        public sealed class PaginationParameters
        {
            public static ValueTask<PaginationParameters?> BindAsync(HttpContext context)
            {
                _ = int.TryParse(context.Request.Query["PageNumber"], out var page);
                _ = int.TryParse(context.Request.Query["PageSize"], out var size);

                var result = new PaginationParameters
                {
                    PageNumber = page,
                    PageSize = size
                };

                return ValueTask.FromResult<PaginationParameters?>(result);
            }

            public int PageNumber { get; set; }
            public int PageSize { get; set; }
        }

        public sealed class FilterByFieldNameParameters
        {
            public static ValueTask<FilterByFieldNameParameters?> BindAsync(HttpContext context)
            {
                _ = int.TryParse(context.Request.Query["PageNumber"], out var page);
                _ = int.TryParse(context.Request.Query["PageSize"], out var size);
                _ = int.TryParse(context.Request.Query["Skip"], out var skip);
                _ = int.TryParse(context.Request.Query["Take"], out var take);

                var result = new FilterByFieldNameParameters
                {
                    SearchField = context.Request.Query["SearchField"],
                    SearchCriteria = context.Request.Query["SearchCriteria"],
                    OrderBy = context.Request.Query["OrderBy"],
                    PageNumber = page,
                    PageSize = size,
                    Skip = skip,
                    Take = take
                };

                return ValueTask.FromResult<FilterByFieldNameParameters?>(result);
            }

            public string? SearchField { get; set; }
            public string? SearchCriteria { get; set; }
            public string? OrderBy { get; set; }
            public int PageNumber { get; set; }
            public int PageSize { get; set; }
            public int Skip { get; set; }
            public int Take { get; set; }
        }                
    }
}