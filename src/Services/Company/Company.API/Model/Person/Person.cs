namespace Awc.Dapr.Services.Company.API.Model.Person
{
    public class Person
    {
        public int BusinessEntityID { get; set; }
        public string? PersonType { get; set; }
        public bool NameStyle { get; set; }
        public string? Title { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
        public int EmailPromotion { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }
        public virtual Password? Password { get; set; }
        public virtual List<EmailAddress> EmailAddresses { get; set; } = [];
        public virtual List<PersonPhone> Telephones { get; set; } = [];
        public virtual List<BusinessEntityAddress> BusinessEntityAddresses { get; set; } = [];        
    }
}