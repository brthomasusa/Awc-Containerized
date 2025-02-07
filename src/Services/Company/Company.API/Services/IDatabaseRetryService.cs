namespace Awc.Services.Company.API.Services
{
    public interface IDatabaseRetryService
    {
        Task ExecuteWithRetryAsync(Func<Task> action);
    }
}