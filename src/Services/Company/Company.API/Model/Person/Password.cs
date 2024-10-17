namespace Awc.Services.Company.API.Model.Person
{
    public sealed class Password
    {
        public int BusinessEntityID { get; set; }
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }        
    }
}