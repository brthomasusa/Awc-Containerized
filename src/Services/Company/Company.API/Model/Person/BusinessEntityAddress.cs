namespace Awc.Services.Company.API.Model.Person
{
    public class BusinessEntityAddress
    {
        public int BusinessEntityID { get; set; }
        public int AddressID { get; set; }
        public virtual Address? Address { get; set; }
        public int AddressTypeID { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }        
    }
}