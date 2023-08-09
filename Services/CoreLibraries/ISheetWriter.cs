namespace CoreLibraries;

public interface ISheetWriter
{
    public bool WriteTo<T>(IEnumerable<T> list, string spreadsheetId, string range, string? header = null)
    where T : class, new();
}
