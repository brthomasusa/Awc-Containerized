namespace Awc.Dapr.Services.Company.API.Model.Person
{
    public sealed class StateProvince
    {
        public int StateProvinceID { get; set; }
        public string? StateProvinceCode { get; set; }
        public string? CountryRegionCode { get; set; }
        public bool IsOnlyStateProvinceFlag { get; set; }
        public string? Name { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }        
    }
}