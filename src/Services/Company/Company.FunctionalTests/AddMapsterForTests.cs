using Mapster;
using MapsterMapper;

namespace Company.FunctionalTests
{
    public static class AddMapsterForTests
    {
        public static Mapper GetMapper()
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(
                typeof(Awc.Services.Company.API.Program).Assembly
            );

            config.Default.NameMatchingStrategy(NameMatchingStrategy.IgnoreCase);

            return new Mapper(config);
        }        
    }
}