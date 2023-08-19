using System.Data;
using MySql.Data.MySqlClient;


namespace Services.CoreLibraries;

public class MySqlDbConnectionManager : IDbConnectionManager
{
    private string _connectionString { get; set; }

    public MySqlDbConnectionManager(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }

}
