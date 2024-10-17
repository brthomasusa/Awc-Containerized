namespace Awc.Dapr.Services.Company.API.Infrastructure.EntityConfigurations.Company
{
    internal class ShiftConfig : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> entity)
        {
            entity.ToTable("Shift", schema: "HumanResources");
            entity.HasKey(e => e.ShiftID);
            entity.HasIndex(p => p.Name).IsUnique();

            entity.Property(e => e.ShiftID)  // tinyint
                .HasColumnName("ShiftID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("nvarchar(50)");
            entity.Property(e => e.StartTime)
                .IsRequired()
                .HasColumnName("StartTime")
                .HasColumnType("time(7)");
            entity.Property(e => e.EndTime)
                .IsRequired()
                .HasColumnName("EndTime")
                .HasColumnType("time(7)");
            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValue(DateTime.Now);
        }
    }
}