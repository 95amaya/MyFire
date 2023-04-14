using System.Collections.Generic;

namespace MyFireCoreLibraries;

public interface ISheetReader
{
    public IList<T> ReadFrom<T>(string spreadsheetId, string range)
    where T: class, new();
}
