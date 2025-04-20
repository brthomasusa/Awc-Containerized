using Awc.Services.Product.Product.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Product.FunctionalTests
{
    public abstract class TestBase : IDisposable
    {
        protected readonly ProductDbContext _dbContext;
        protected readonly DapperContext _dapperCtx;

        protected TestBase()
        {
            string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ProductDbTest");
            var optionsBuilder = new DbContextOptionsBuilder<ProductDbContext>();

            optionsBuilder.UseSqlServer(
                connectionString!,
                msSqlOptions => msSqlOptions.MigrationsAssembly(typeof(ProductDbContext).Assembly.FullName)
            )
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();

            _dbContext = new ProductDbContext(optionsBuilder.Options);
            _dapperCtx = new DapperContext(connectionString!);

            // _dbContext.Database.ExecuteSqlRaw("EXEC dbo.usp_InitializeTestDb");
            _ = ReseedTestDatabase.ReseedDatabase();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}