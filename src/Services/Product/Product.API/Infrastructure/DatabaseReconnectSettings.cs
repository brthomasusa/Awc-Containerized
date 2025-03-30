namespace Awc.Services.Product.Product.API.Infrastructure
{
    public class DatabaseReconnectSettings
    {
        public int RetryCount { get; set; }
        public int RetryWaitPeriodInSeconds { get; set; }
    }
}