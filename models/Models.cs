using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace MyFire.Models
{
    public class ColumnMapAttribute: Attribute
    {
        private char _Col { get; set; }
        public ColumnMapAttribute(char column)
        {
            _Col = column;
        }
        public virtual char Column { get { return _Col; }}
    }
    public class Test
    {
        [ColumnMap('A')]
        public string Foo {get; set;}
        [ColumnMap('B')]
        public string Bar {get; set;}
    }

    public abstract class GoogleSheetModel
    {
        public abstract object[] GetVals { get; }
    }
    public class CmpIntRow: GoogleSheetModel
    {

        [ColumnMap('A')]
        public int Year {get; set;}
        public double CurrentVal {get; set;}
        [ColumnMap('B')]
        public double InterestVal {get; set;}

        [ColumnMap('D')]
        public double TotalVal {get; set;}

        public override object[] GetVals
        { 
            get 
            { 
                return new object[]{Year, CurrentVal, InterestVal, TotalVal };
            }
        }
    }
}