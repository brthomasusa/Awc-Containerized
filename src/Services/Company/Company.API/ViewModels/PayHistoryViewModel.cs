namespace Awc.Services.Company.API.ViewModels
{
    public sealed class PayHistoryViewModel
    {
        public int BusinessEntityID { get; set; }
        public DateTime RateChangeDate { get; set; }
        public decimal PayRate { get; set; }       
    }
}