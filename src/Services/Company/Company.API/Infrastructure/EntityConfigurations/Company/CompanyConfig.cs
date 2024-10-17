using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Awc.Services.Company.API.Infrastructure.EntityConfigurations.Company
{
    internal class CompanyConfig : IEntityTypeConfiguration<Awc.Services.Company.API.Model.Company.Company>
    {
        public void Configure(EntityTypeBuilder<Awc.Services.Company.API.Model.Company.Company> entity)
        {
            entity.ToTable("Company", schema: "Person");
            entity.HasKey(e => e.CompanyID);

            entity.Property(e => e.CompanyID)
                .HasColumnName("CompanyID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.CompanyName)
                .IsRequired()
                .HasColumnName("CompanyName")
                .HasColumnType("nvarchar(50)");
            entity.Property(e => e.LegalName)
                .HasColumnName("LegalName")
                .HasColumnType("nvarchar(50)")
                .HasDefaultValue("Same as company name");
            entity.Property(e => e.EIN)
                .IsRequired()
                .HasColumnName("EIN")
                .HasColumnType("nchar(9)");
            entity.Property(e => e.WebsiteUrl)
                .HasColumnName("WebsiteUrl")
                .HasColumnType("nvarchar(50)");
            entity.Property(e => e.MailAddressLine1)
                .IsRequired()
                .HasColumnName("MailAddressLine1")
                .HasColumnType("nvarchar(60)");
            entity.Property(e => e.MailAddressLine2)
                .HasColumnName("MailAddressLine2")
                .HasColumnType("nvarchar(60)");
            entity.Property(e => e.MailCity)
                .IsRequired()
                .HasColumnName("MailCity")
                .HasColumnType("nvarchar(30)");
            entity.Property(e => e.MailStateProvinceID)
                .HasColumnName("MailStateProvinceID")
                .IsRequired();
            entity.Property(e => e.MailPostalCode)
                .IsRequired()
                .HasColumnName("MailPostalCode")
                .HasColumnType("nvarchar(60)");
            entity.Property(e => e.DeliveryAddressLine1)
                .IsRequired()
                .HasColumnName("DeliveryAddressLine1")
                .HasColumnType("nvarchar(60)");
            entity.Property(e => e.DeliveryAddressLine2)
                .HasColumnName("DeliveryAddressLine2")
                .HasColumnType("nvarchar(60)");
            entity.Property(e => e.DeliveryCity)
                .IsRequired()
                .HasColumnName("DeliveryCity")
                .HasColumnType("nvarchar(30)");
            entity.Property(e => e.DeliveryStateProvinceID)
                .HasColumnName("DeliveryStateProvinceID")
                .IsRequired();
            entity.Property(e => e.DeliveryPostalCode)
                .IsRequired()
                .HasColumnName("DeliveryPostalCode")
                .HasColumnType("nvarchar(60)");
            entity.Property(e => e.Telephone)
                .IsRequired()
                .HasColumnName("Telephone")
                .HasColumnType("nvarchar(25)");
            entity.Property(e => e.Fax)
                .IsRequired()
                .HasColumnName("Fax")
                .HasColumnType("nvarchar(25)");
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
}