namespace Awc.Services.Product.Product.API.Infrastructure;

public class DapperContext(string connStr)
{
    private readonly string _connectionStr = connStr;

    public IDbConnection CreateConnection() => new SqlConnection(_connectionStr);
}
