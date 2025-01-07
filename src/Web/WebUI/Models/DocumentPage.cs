namespace WebUI.Models
{
    public class DocumentPage<T>(IEnumerable<T> data, int total, int totalPages, int currentPage)
    {
        public int Total { get; } = total;

        public int TotalPages { get; } = totalPages;

        public int CurrentPage { get; } = currentPage;

        public IEnumerable<T> Data { get; } = data;
    }
}