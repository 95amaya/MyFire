using Google.Apis.Sheets.v4;
using MyFireCoreLibraries;

namespace MyFire.Services
{
    public class GoogleSheetWriter: ISheetWriter
    {
        private SheetsService GoogleSheetsApiService { get; set; }
        public GoogleSheetWriter(SheetsService googleSheetsApiService)
        {
            GoogleSheetsApiService = googleSheetsApiService;
        }

        public bool WriteTo<T>(IEnumerable<T> list, string spreadsheetId, string range, string? header = null) where T : class, new()
        {
            throw new NotImplementedException();
        }


        // public UpdateValuesResponse WriteSheet<T>(IEnumerable<T> list, string spreadsheetId, string range, string header = null)
        //     where T: GoogleSheetModel
        // {
        //     var request = GoogleSheetsApiService.Spreadsheets.Values.Update(list.GoogleSheetsFormat(range, header), spreadsheetId, range);
        //     request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
        //     request.IncludeValuesInResponse = true;
        //     var response = request.Execute();
        //     return response;
        // }

    }
    
}