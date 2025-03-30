using System.Reflection;

namespace Awc.Services.Product.Product.API
{
    public static class ServerAssembly
    {
        public static readonly Assembly Instance = typeof(ServerAssembly).Assembly;
    }
}