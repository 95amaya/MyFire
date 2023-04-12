using System;

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

    public class Student
    {
        public string FirstName {get; set;}
        public string Sex {get; set;}
        public string Class {get; set;}
        public string City {get; set;}
        public string Major {get; set;}

        public override string ToString()
        {
            return $"{FirstName}, {Major}, {City}";
        }
    }
}