#pragma warning disable CS8604

using System.Reflection;

namespace Awc.Services.Product.Product.API.Infrastructure
{
    public class ProductDbContext(DbContextOptions options) : DbContext(options)
    {


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
            => await base.SaveChangesAsync(cancellationToken);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}