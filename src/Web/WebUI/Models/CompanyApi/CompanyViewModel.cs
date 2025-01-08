namespace WebUI.Models.CompanyApi
{
    public class CompanyViewModel
    {
        public int CompanyID { get; set; }
        public string? CompanyName { get; set; }
        public string? LegalName { get; set; }
        public string? EIN { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? MailAddressLine1 { get; set; }
        public string? MailAddressLine2 { get; set; }
        public string? MailCity { get; set; }
        public string? MailState { get; set; }
        public string? MailPostalCode { get; set; }
        public string? DeliveryAddressLine1 { get; set; }
        public string? DeliveryAddressLine2 { get; set; }
        public string? DeliveryCity { get; set; }
        public string? DeliveryState { get; set; }
        public string? DeliveryPostalCode { get; set; }
        public string? Telephone { get; set; }
        public string? Fax { get; set; }
        public List<DepartmentViewModel> Departments { get; set; } = [];
        public List<ShiftViewModel> Shifts { get; set; } = [];

        public string FullMailAddress
        {
            get => $"{MailAddressLine1!} {MailAddressLine2!}, {MailCity!}, {MailState} {MailPostalCode!}";
            set { }
        }

        public string FullDeliveryAddress
        {
            get => $"{DeliveryAddressLine1!} {DeliveryAddressLine2!}, {DeliveryCity!}, {DeliveryState} {DeliveryPostalCode!}";
            set { }
        }
    }
}