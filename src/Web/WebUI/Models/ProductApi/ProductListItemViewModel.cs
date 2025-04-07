using WebUI.Utilities;
using System.Text.Json.Serialization;

namespace WebUI.Models.ProductApi
{
    public sealed class ProductListItemViewModel
    {
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public string? ProductNumber { get; set; }

        // [JsonConverter(typeof(BitToBooleanJsonConverter))]
        public bool MakeFlag { get; set; }

        // [JsonConverter(typeof(BitToBooleanJsonConverter))]
        public bool FinishedGoodsFlag { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public int DaysToManufacture { get; set; }
        public string? ProductSubCategory { get; set; }
        public string? ProductModel { get; set; }

        // [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime SellStartDate { get; set; }

        // [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime SellEndDate { get; set; }

        // [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime DiscontinuedDate { get; set; }
    }
}