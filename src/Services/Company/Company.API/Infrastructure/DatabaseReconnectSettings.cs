namespace Awc.Services.Company.API.Infrastructure
{
    public class DatabaseReconnectSettings
    {
        public int RetryCount { get; set; }
        public int RetryWaitPeriodInSeconds { get; set; }
    }
}