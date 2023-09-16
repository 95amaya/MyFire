namespace CoreLibraries;

public interface ISheetReader
{
    public IList<T> Read<T>(string spreadsheetId, string range)
    where T : class, new();
}
