namespace CoreLibraries;

public interface ISheetClient
{
    public IList<IList<object>> GetValues(string spreadsheetId, string range);
}
