namespace Awc.Services.Company.API
{
    public sealed class SettingsOptions
    {
        public string? CompanyDbConnectionString { get; set; }
        public string? RedisConnectionString { get; set; }
        public string? Environment { get; set; }
    }
}