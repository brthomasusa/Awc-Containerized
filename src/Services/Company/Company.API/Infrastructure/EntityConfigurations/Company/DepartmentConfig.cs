namespace Awc.Services.Company.API.Infrastructure.EntityConfigurations.Company
{
    internal class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.ToTable("Department", schema: "HumanResources");
            entity.HasKey(e => e.DepartmentID);
            entity.HasIndex(p => p.Name).IsUnique();

            entity.Property(e => e.DepartmentID)
                .HasColumnName("DepartmentID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("nvarchar(50)");
            entity.Property(e => e.GroupName)
                .IsRequired()
                .HasColumnName("GroupName")
                .HasColumnType("nvarchar(50)");
            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValue(DateTime.Now);
        }
    }
}