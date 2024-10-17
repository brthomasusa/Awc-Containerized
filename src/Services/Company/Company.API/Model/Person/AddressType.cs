namespace Awc.Dapr.Services.Company.API.Model.Person
{
    public class AddressType
    {
        public int AddressTypeID { get; set; }
        public string? Name { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }        
    }
}