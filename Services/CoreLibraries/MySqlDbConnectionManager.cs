using System.Data;
using MySql.Data.MySqlClient;


namespace Services.CoreLibraries;

public class MySqlDbConnectionManager : IDbConnectionManager
{
    private readonly string _connectionString;

    public MySqlDbConnectionManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }

}
