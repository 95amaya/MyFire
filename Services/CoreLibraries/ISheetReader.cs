namespace CoreLibraries;

public interface ISheetReader
{
    public IList<T> ReadFrom<T>(string spreadsheetId, string range)
    where T : class, new();
}
