using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyFire.Models;
using System;

namespace MyFire.Services
{
    public class GoogleSheetWriter: ISheetWriter
    {
        private SheetsService GoogleSheetsApiService { get; set; }
        private char[] AlphRange { get; set; }
        public GoogleSheetWriter(SheetsService googleSheetsApiService, char[] alphaRange)
        {
            GoogleSheetsApiService = googleSheetsApiService;
            AlphRange = alphaRange;
        }

        public bool WriteTo<T>(IEnumerable<T> list, string spreadsheetId, string range, string header = null) where T : class, new()
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