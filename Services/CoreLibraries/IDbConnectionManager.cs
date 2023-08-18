using System.Data;

namespace Services.CoreLibraries;

public interface IDbConnectionManager
{
    public IDbConnection CreateConnection();
}
