using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyFire.Models;
using System;
using AutoMapper;

namespace MyFire.Services
{
    public class GoogleSheetClient: ISheetClient
    {
        private SheetsService _googleSheetsApiClient { get; set; }

        public GoogleSheetClient(SheetsService googleSheetsApiClient)
        {
            _googleSheetsApiClient = googleSheetsApiClient;
        }

        public IList<IList<Object>> GetValues(string spreadsheetId, string range) 
        {
            var request = _googleSheetsApiClient.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();
            return response.Values;
        }
    }
    
}