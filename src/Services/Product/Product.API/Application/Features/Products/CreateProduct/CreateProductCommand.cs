using Awc.Services.Product.Product.API.Application.Abstractions.Messaging;

namespace Awc.Services.Product.Product.API.Application.Features.Products.CreateProduct
{
    public record CreateProductCommand
    (
        int ProductID,
        string Name,
        string ProductNuber,
        bool MakeFlag,
        bool FinishedGoodsFlag,
        string Color,
        Int16 SafetyStockLevel,
        Int16 ReorderPoint,
        decimal StandardCost,
        decimal ListPrice,
        string Size,
        string SizeUnitMeasureCode,
        string WeightUnitMeasureCode,
        decimal Weight,
        int DaysToManufacture,
        string ProductLine,
        string Class,
        string Style,
        int ProductSubcategoryID,
        int ProductModelID,
        DateTime SellStartDate,
        DateTime SellEndDate,
        DateTime DiscontinuedDate
    ) : ICommand<int>;
}