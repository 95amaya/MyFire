using Google.Apis.Sheets.v4;

namespace CoreLibraries;

public class GoogleSheetClient : ISheetClient
{
    private SheetsService _googleSheetsApiClient { get; set; }

    public GoogleSheetClient(SheetsService googleSheetsApiClient)
    {
        _googleSheetsApiClient = googleSheetsApiClient;
    }

    public IList<IList<object>> GetValues(string spreadsheetId, string range)
    {
        var request = _googleSheetsApiClient.Spreadsheets.Values.Get(spreadsheetId, range);
        var response = request.Execute();
        return response.Values;
    }

}