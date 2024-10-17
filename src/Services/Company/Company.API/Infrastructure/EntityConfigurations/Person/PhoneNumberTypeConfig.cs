using Awc.Dapr.Services.Company.API.Model.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awc.Dapr.Services.Company.API.Infrastructure.EntityConfigurations.Person;

internal class PhoneNumberTypeConfig : IEntityTypeConfiguration<PhoneNumberType>
{
    public void Configure(EntityTypeBuilder<PhoneNumberType> entity)
    {
        entity.ToTable("PhoneNumberType", schema: "Person");
        entity.HasKey(e => e.PhoneNumberTypeID);
        entity.HasIndex(p => p.Name).IsUnique();

        entity.Property(e => e.PhoneNumberTypeID)
            .HasColumnName("PhoneNumberTypeID")
            .ValueGeneratedOnAdd();
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
