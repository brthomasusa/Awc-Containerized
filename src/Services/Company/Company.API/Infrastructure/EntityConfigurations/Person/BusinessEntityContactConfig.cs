using Awc.Services.Company.API.Model.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awc.Services.Company.API.Infrastructure.EntityConfigurations.Person;

internal class BusinessEntityContactConfig : IEntityTypeConfiguration<BusinessEntityContact>
{
    public void Configure(EntityTypeBuilder<BusinessEntityContact> entity)
    {
        entity.ToTable("BusinessEntityContact", schema: "Person");
        entity.HasKey(e => new { e.BusinessEntityID, e.PersonID, e.ContactTypeID });
        entity.HasOne<Awc.Services.Company.API.Model.Person.Person>()
            .WithMany()
            .HasForeignKey(p => p.PersonID)
            .IsRequired();
        entity.HasOne<ContactType>()
            .WithMany()
            .HasForeignKey(p => p.ContactTypeID)
            .IsRequired();

        entity.Property(e => e.BusinessEntityID)
            .HasColumnName("BusinessEntityID")
            .ValueGeneratedNever();
        entity.Property(e => e.PersonID)
            .HasColumnName("PersonID");
        entity.Property(e => e.ContactTypeID)
            .HasColumnName("ContactTypeID");
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
