using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyFire.Models;
using System;

namespace MyFire.Services
{
    public class GoogleSheetReader: ISheetReader
    {
        private SheetsService GoogleSheetsApiService { get; set; }
        private char[] AlphRange { get; set; }
        public GoogleSheetReader(SheetsService googleSheetsApiService, char[] alphaRange)
        {
            GoogleSheetsApiService = googleSheetsApiService;
            AlphRange = alphaRange;
        }

        public UpdateValuesResponse WriteSheet<T>(IEnumerable<T> list, string spreadsheetId, string range, string header = null)
            where T: GoogleSheetModel
        {
            var request = GoogleSheetsApiService.Spreadsheets.Values.Update(list.GoogleSheetsFormat(range, header), spreadsheetId, range);
            request.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            request.IncludeValuesInResponse = true;
            var response = request.Execute();
            return response;
        }

        public IEnumerable<T> ReadSheet<T>(string spreadsheetId, string range)
        where T: class, new() 
        {
            var retVal = new List<T>();
            var request = GoogleSheetsApiService.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();
            if(response.Values.SafeHasRows())
            {
                var objColArr = GetColMap(range, Extensions.GetPropMapDict<T>());
                
                foreach(var row in response.Values)
                {
                    var mappingObj = new T();
                    var rowArr = row.ToArray();
                    for(int col = 0; col < rowArr.Length; col++)
                    {
                        var prop = objColArr[col];
                        var val = rowArr[col];
                        if(val != null)
                        {
                            if(prop.PropertyType == typeof(string))
                            {
                                prop.SetValue(mappingObj, val);
                            }
                            else
                            {
                                prop.SetValue(mappingObj, Convert.ChangeType(val, prop.PropertyType));
                            }
                        }
                    }
                    retVal.Add(mappingObj);
                }
            }
            return retVal;
        }

        private PropertyInfo[] GetColMap(string range, Dictionary<char, PropertyInfo> propMap)
        {
            if(!propMap.SafeHasRows())
            {
                throw new ArgumentNullException(nameof(propMap));
            }
            var retVal = new List<PropertyInfo>();
            var colRange = new List<char>();
            var cleanedRange = range.Split(":").Select(p => p.Trim().ToUpper().FirstOrDefault()).ToList();
            char startCol = cleanedRange.FirstOrDefault();
            char endCol = cleanedRange.LastOrDefault();
            
            colRange.AddRange(AlphRange.SkipWhile(val => !val.Equals(startCol)));
            if(startCol != endCol && endCol > startCol)
            {
                colRange = colRange.TakeWhile(val => !val.Equals(endCol)).ToList();
                colRange.Add(endCol);
            }
            else
            {
                colRange = colRange.Take(1).ToList();
            }

            return colRange.Select(col => propMap[col]).ToArray();
        }
    }
    
}