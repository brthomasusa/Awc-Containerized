using System.Data.SqlClient;
namespace Product.FunctionalTests
{
    public static class ReseedTestDatabase
    {
        public static Result<bool> ReseedDatabase()
        {
            string? _connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__ProductDbTest");

            try
            {
                using SqlConnection connection = new(_connectionString);
                SqlCommand command = new("dbo.usp_InitializeTestDb", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure<bool>(new Error("ReseedTestDatabase.ReseedTestDatabase", Helpers.GetInnerExceptionMessage(ex)));
            }
        }
    }
}