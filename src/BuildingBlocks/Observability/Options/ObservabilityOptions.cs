namespace Awc.BuildingBlocks.Observability.Options
{
    public sealed class ObservabilityOptions
    {
        public string ServiceName { get; set; } = default!;
        public string CollectorUrl { get; set; } = "http://localhost:4317";
        public string SeqUrl { get; set; } = "http://localhost:5341";

        public bool EnabledTracing { get; set; } = false; //
        public bool EnabledSqlClientTracing { get; set; } = false;
        public bool EnabledHttpClientTracing { get; set; } = false;
        public bool EnabledEfCoreTracing { get; set; } = false;
        public bool EnabledMetrics { get; set; } = false;

        public Uri CollectorUri => new(this.CollectorUrl);

        public string OtlpLogsCollectorUrl => $"{this.CollectorUrl}/v1/logs";        
    }
}