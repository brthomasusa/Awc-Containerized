namespace Awc.Services.Product.Product.API
{
    public sealed class SettingsOptions
    {
        public string? ProductDbConnectionString { get; set; }
        public string? RedisConnectionString { get; set; }
        public string? Environment { get; set; }
    }
}