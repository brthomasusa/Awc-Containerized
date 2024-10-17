using Awc.Dapr.Services.Company.API.Model.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awc.Dapr.Services.Company.API.Infrastructure.EntityConfigurations.Person;

internal class BusinessEntityConfig : IEntityTypeConfiguration<BusinessEntity>
{
    public void Configure(EntityTypeBuilder<BusinessEntity> entity)
    {
        entity.ToTable("BusinessEntity", schema: "Person");
        entity.HasKey(e => e.BusinessEntityID);
        entity.HasOne(p => p.PersonModel)
            .WithOne()
            .HasForeignKey<Awc.Dapr.Services.Company.API.Model.Person.Person>(p => p.BusinessEntityID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        entity.Property(e => e.BusinessEntityID)
            .HasColumnName("BusinessEntityID")
            .ValueGeneratedOnAdd();
        entity.Property(e => e.RowGuid)
            .HasColumnName("rowguid")
            .HasColumnType("UNIQUEIDENTIFIER")
            .IsRequired()
            .HasDefaultValue(Guid.NewGuid());
        entity.Property(e => e.ModifiedDate)
            .HasColumnName("ModifiedDate")
            .IsRequired()
            .HasDefaultValue(DateTime.Now);
    }
}
