using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Google.Apis.Sheets.v4.Data;
using MyFire.Models;

namespace MyFire
{
    public static class Extensions
    {
        public static bool SafeHasRows<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }
        public static bool SafeHasRows<T>(this IList<T> list)
        {
            return list != null && list.Any();
        }

        public static ValueRange GoogleSheetsFormat<T>(this IEnumerable<T> list, string range, string header = null)
            where T: GoogleSheetModel
        {
            var retVal = new ValueRange();
            retVal.Range = range;
            retVal.MajorDimension = "ROWS";

            if(list.SafeHasRows())
            {
                var valList = new List<IList<object>>();
                if(header.SafeHasRows())
                {
                    valList.Add((IList<object>)header.Split(','));
                }
                valList.AddRange(list.Select(p => p.GetVals));
                retVal.Values = valList; 
            }
            return retVal;
        }
        public static Dictionary<char, string> GetPropMapNameDict<T>()
        {
            return GetPropMapDict<T>().ToDictionary(p => p.Key, p => p.Value.Name);
        }
        public static Dictionary<char, PropertyInfo> GetPropMapDict<T>()
        {
            var retVal = new Dictionary<char, PropertyInfo>();
            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if(props.SafeHasRows())
            {
                foreach(var propInfo in props)
                {
                    var attr = (ColumnMapAttribute)propInfo.GetCustomAttribute(typeof(ColumnMapAttribute));
                    if(attr != null)
                    {
                        retVal.Add(attr.Column, propInfo);
                    }
                }
            }
            return retVal;
        }

    }
}