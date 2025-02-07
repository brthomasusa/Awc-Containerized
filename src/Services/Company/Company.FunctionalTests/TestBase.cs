using Awc.Services.Company.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Moq;

namespace Company.FunctionalTests
{
    public abstract class TestBase : IDisposable
    {
        protected readonly CompanyDbContext _dbContext;
        protected readonly DapperContext _dapperCtx;

        protected TestBase()
        {
            string? connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__CompanyDbAzure");
            var optionsBuilder = new DbContextOptionsBuilder<CompanyDbContext>();

            optionsBuilder.UseSqlServer(
                connectionString!,
                msSqlOptions => msSqlOptions.MigrationsAssembly(typeof(CompanyDbContext).Assembly.FullName)
            )
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();

            _dbContext = new CompanyDbContext(optionsBuilder.Options);
            _dapperCtx = new DapperContext(connectionString!);

            // Company.FunctionalTests is only testing queries so no need to reset the database.
            // _dbContext.Database.ExecuteSqlRaw("EXEC dbo.usp_InitializeTestDb");
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}