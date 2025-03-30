namespace Awc.Services.Product.Product.API.Services
{
    public interface IDatabaseRetryService
    {
        Task ExecuteWithRetryAsync(Func<Task> action);
    }
}