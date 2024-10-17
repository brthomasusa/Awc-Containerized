namespace Awc.Services.Company.API.Infrastructure.EntityConfigurations.Employee
{
    internal class EmployeeManagerConfig : IEntityTypeConfiguration<EmployeeManager>
    {
        public void Configure(EntityTypeBuilder<EmployeeManager> entity)
        {
            entity.ToView("vGetManagersBasicInfo", schema: "HumanResources");
            entity.HasNoKey();

            entity.Property(e => e.BusinessEntityID)
                .HasColumnName("BusinessEntityID");
            entity.Property(e => e.DepartmentID)
                .HasColumnName("DepartmentID");
            entity.Property(e => e.DepartmentName)
                .HasColumnName("DepartmentName");
            entity.Property(e => e.FirstName)
                .HasColumnName("FirstName");
            entity.Property(e => e.MiddleName)
                .HasColumnName("MiddleName");
            entity.Property(e => e.LastName)
                .HasColumnName("LastName");
        }
    }
}