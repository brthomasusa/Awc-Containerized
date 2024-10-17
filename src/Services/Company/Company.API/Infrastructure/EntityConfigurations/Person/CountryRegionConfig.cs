using Awc.Services.Company.API.Model.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awc.Services.Company.API.Infrastructure.EntityConfigurations.Person;

internal class CountryRegionConfig : IEntityTypeConfiguration<CountryRegion>
{
    public void Configure(EntityTypeBuilder<CountryRegion> entity)
    {
        entity.ToTable("CountryRegion", schema: "Person");
        entity.HasKey(e => e.CountryRegionCode);
        entity.HasIndex(p => p.Name).IsUnique();

        entity.Property(e => e.CountryRegionCode)
            .HasColumnName("CountryRegionCode")
            .HasColumnType("nvarchar(3)")
            .ValueGeneratedNever();
        entity.Property(e => e.Name)
            .IsRequired()
            .HasColumnName("Name")
            .HasColumnType("nvarchar(50)");
        entity.Property(e => e.ModifiedDate)
            .HasColumnName("ModifiedDate")
            .IsRequired()
            .HasDefaultValue(DateTime.Now);
    }
}
