namespace WebUI.Models
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