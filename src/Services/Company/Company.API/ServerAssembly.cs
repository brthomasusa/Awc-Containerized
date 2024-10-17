using System.Reflection;

namespace Awc.Dapr.Services.Company.API
{
    public static class ServerAssembly
    {
        public static readonly Assembly Instance = typeof(ServerAssembly).Assembly;
    }
}