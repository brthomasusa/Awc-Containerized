using Awc.Services.Company.API.Model.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awc.Services.Company.API.Infrastructure.EntityConfigurations.Person;

internal class BusinessEntityAddressConfig : IEntityTypeConfiguration<BusinessEntityAddress>
{
    public void Configure(EntityTypeBuilder<BusinessEntityAddress> entity)
    {
        entity.ToTable("BusinessEntityAddress", schema: "Person");
        entity.HasKey(e => new { e.BusinessEntityID, e.AddressID, e.AddressTypeID });
        entity.HasOne(businessEntityAddress => businessEntityAddress.Address)
            .WithMany()
            .HasForeignKey(businessEntityAddress => businessEntityAddress.AddressID)
            .IsRequired();

        entity.Property(e => e.BusinessEntityID)
            .HasColumnName("BusinessEntityID")
            .ValueGeneratedNever();
        entity.Property(e => e.AddressID)
            .HasColumnName("AddressID");
        entity.Property(e => e.AddressTypeID)
            .HasColumnName("AddressTypeID");
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
