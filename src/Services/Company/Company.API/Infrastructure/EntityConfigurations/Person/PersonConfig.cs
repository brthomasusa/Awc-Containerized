using Awc.Services.Company.API.Model.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Awc.Services.Company.API.Infrastructure.EntityConfigurations.Person;

internal class PersonModelConfig : IEntityTypeConfiguration<Awc.Services.Company.API.Model.Person.Person>
{
    public void Configure(EntityTypeBuilder<Awc.Services.Company.API.Model.Person.Person> entity)
    {
        entity.ToTable("Person", schema: "Person");
        entity.HasKey(e => e.BusinessEntityID);

        entity.HasOne(p => p.Password)
            .WithOne()
            .HasForeignKey<Password>(p => p.BusinessEntityID)
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasMany(p => p.EmailAddresses)
            .WithOne()
            .HasForeignKey(p => p.BusinessEntityID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasMany(p => p.Telephones)
            .WithOne()
            .HasForeignKey(p => p.BusinessEntityID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        entity.HasMany(p => p.BusinessEntityAddresses)
            .WithOne()
            .HasForeignKey(p => p.BusinessEntityID)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        entity.Property(e => e.BusinessEntityID)
            .HasColumnName("BusinessEntityID")
            .ValueGeneratedNever();
        entity.Property(e => e.PersonType)
            .IsRequired()
            .HasColumnName("PersonType")
            .HasColumnType("nchar(2)");
        entity.Property(e => e.NameStyle)
            .HasColumnName("NameStyle")
            .HasColumnType("bit")
            .IsRequired()
            .HasDefaultValue(0);
        entity.Property(e => e.Title)
            .HasColumnName("Title")
            .HasColumnType("nvarchar(8)");
        entity.Property(e => e.FirstName)
            .IsRequired()
            .HasColumnName("FirstName")
            .HasColumnType("nvarchar(50)");
        entity.Property(e => e.MiddleName)
            .HasColumnName("MiddleName")
            .HasColumnType("nvarchar(50)");
        entity.Property(e => e.LastName)
            .IsRequired()
            .HasColumnName("LastName")
            .HasColumnType("nvarchar(50)");
        entity.Property(e => e.Suffix)
            .HasColumnName("Suffix")
            .HasColumnType("nvarchar(8)");
        entity.Property(e => e.EmailPromotion)
            .HasColumnName("EmailPromotion")
            .HasColumnType("int")
            .IsRequired()
            .HasDefaultValue(0);
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
