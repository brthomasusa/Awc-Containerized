namespace Awc.Services.Product.Product.API.Application.Abstractions
{
    public sealed record StringSearchCriteria
    (
        string? SearchField,
        string? SearchCriteria,
        string? OrderBy,
        int Skip,
        int Take
    );
}