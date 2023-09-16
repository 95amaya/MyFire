namespace Services.CoreLibraries;

public interface ICsvReader
{
    public IList<T> Read<T>(string path, bool skipFirstRow = false) where T : class, new();
}
