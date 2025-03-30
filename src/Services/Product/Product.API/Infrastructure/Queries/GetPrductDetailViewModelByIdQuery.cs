using Awc.Services.Product.Product.API.ViewModels;
using Dapper;

namespace Awc.Services.Product.Product.API.Infrastructure.Queries
{
    public class GetPrductDetailViewModelByIdQuery
    {
        public async static Task<Result<ProductDetailViewModel>> DoQuery(DapperContext context, int productId)
        {
            Serilog.ILogger log = Log.ForContext<GetPrductDetailViewModelByIdQuery>();

            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("ID", productId, DbType.Int32);

                string sql =
                @"SELECT            
					ProductID, 
					Name, 
					ProductNumber, 
					MakeFlag, 
					FinishedGoodsFlag, 
					Color, 
					SafetyStockLevel, 
					ReorderPoint, 
					StandardCost,
					ListPrice, 
					Size, 
					SizeUnitOfMeasurement, 
					WeightUnitOfMeasurement, 
					Weight, 
					DaysToManufacture, 
					ProductLine, 
					Class,
					Style, 
					ProductSubCategory, 
					ProductModel, 
					SellStartDate, 
					SellEndDate, 
					DiscontinuedDate                       
                FROM Production.vProductDetailViewModel
                WHERE ProductID = @ID";

                using var conn = context.CreateConnection();
                var model = await conn.QueryFirstOrDefaultAsync<ProductDetailViewModel>(sql, parameters);

                if (model is null)
                {
                    string errMsg = $"Unable to retrieve details for product with ID: {productId}.";
                    log.Warning(errMsg, productId);

                    return Result<ProductDetailViewModel>.Failure<ProductDetailViewModel>(
                        new Error("GetPrductDetailViewModelByIdQuery.DoQuery", errMsg)
                    );
                }

                return model;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred: {ErrorMessage}", Helpers.GetInnerExceptionMessage(ex));

                return Result<ProductDetailViewModel>.Failure<ProductDetailViewModel>(
                    new Error("GetPrductDetailViewModelByIdQuery.DoQuery", Helpers.GetInnerExceptionMessage(ex)));
            }
        }
    }
}