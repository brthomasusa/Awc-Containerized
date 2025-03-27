using WebUI.Utilities;
using System.Text.Json.Serialization;

namespace WebUI.Models.ProductApi
{
    public sealed class ProductDetailViewModel
    {
        public int ProductID { get; set; }
        public string? Name { get; set; }
        public string? ProductNumber { get; set; }

        [JsonConverter(typeof(BitToBooleanJsonConverter))]
        public bool MakeFlag { get; set; }

        [JsonConverter(typeof(BitToBooleanJsonConverter))]
        public bool FinishedGoodsFlag { get; set; }
        public string? Color { get; set; }
        public Int16 SafetyStockLevel { get; set; }
        public Int16 ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public string? Size { get; set; }
        public string? SizeUnitOfMeasurement { get; set; }
        public string? WeightUnitOfMeasurement { get; set; }
        // public decimal Weight { get; set; }
        public int DaysToManufacture { get; set; }
        public string? ProductLine { get; set; }
        public string? Class { get; set; }
        public string? Style { get; set; }
        public string? ProductSubCategory { get; set; }
        public string? ProductModel { get; set; }

        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime SellStartDate { get; set; }

        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime SellEndDate { get; set; }

        [JsonConverter(typeof(DateTimeJsonConverter))]
        public DateTime DiscontinuedDate { get; set; }
    }
}