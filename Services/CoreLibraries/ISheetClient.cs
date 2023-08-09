using System;
using System.Collections.Generic;

namespace CoreLibraries;

public interface ISheetClient
{
    public IList<IList<Object>> GetValues(string spreadsheetId, string range);
}
