using System;
using System.Collections.Generic;

namespace MyFireCoreLibraries;

public interface ISheetClient
{
    public IList<IList<Object>> GetValues(string spreadsheetId, string range);
}
