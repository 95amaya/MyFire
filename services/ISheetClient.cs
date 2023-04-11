using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyFire.Models;
using System;

namespace MyFire.Services
{
    public interface ISheetClient
    {
        public IEnumerable<T> GetSheetValues<T>(string spreadsheetId, string range)
        where T: class, new();
    }
    
}