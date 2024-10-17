namespace Awc.Services.Company.API.Model.Person
{
    public class BusinessEntity
    {
        public int BusinessEntityID { get; set; }
        public virtual Person? PersonModel { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }        
    }
}