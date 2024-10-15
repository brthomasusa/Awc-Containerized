namespace Devspaces.Support;

public class DevspacesMessageHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    private const string DevspacesHeaderName = "azds-route-as";
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var req = _httpContextAccessor.HttpContext.Request;

        if (req.Headers.TryGetValue(DevspacesHeaderName, out Microsoft.Extensions.Primitives.StringValues value))
        {
            request.Headers.Add(DevspacesHeaderName, value as IEnumerable<string>);
        }
        return base.SendAsync(request, cancellationToken);
    }
}
