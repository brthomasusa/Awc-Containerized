using System.Data;
using Microsoft.Data.SqlClient;

namespace Awc.Services.Company.API.Infrastructure;

public class DapperContext(string connStr)
{
    private readonly string _connectionStr = connStr;

    public IDbConnection CreateConnection() => new SqlConnection(_connectionStr);
}
