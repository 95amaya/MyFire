namespace Services.CoreLibraries;

public interface ICsvReader
{
    public IList<T> Read<T>(StreamReader reader, bool skipFirstRow = false) where T : class, new();
}
