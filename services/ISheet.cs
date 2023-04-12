using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyFire.Models;
using System;

namespace MyFire.Services
{
    public interface ISheetReader
    {
        public IList<T> ReadFrom<T>(string spreadsheetId, string range)
        where T: class, new();
    }
    public interface ISheetWriter
    {
        public bool WriteTo<T>(IEnumerable<T> list, string spreadsheetId, string range, string header = null)
        where T: class, new();
    }
    public interface ISheetClient
    {
        public IList<IList<Object>> GetValues(string spreadsheetId, string range);
    }
}