using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System.Collections.Generic;
using System.Linq;
using MyFire.Models;
using System;

namespace MyFire.Services
{
    public class FinanceService
    {
        public FinanceService()
        {
            
        }
        
        public IEnumerable<CmpIntRow> BuildCompoundInterestTable(double initVal, double interestRate, int periods, double contr = 0)
        {
            var retVal = new List<CmpIntRow>();
            if(initVal <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initVal));
            }
            var currVal = initVal;
            for(int period = 0; period <= periods; period++)
            {
                var row = new CmpIntRow()
                {
                    Year = period,
                    CurrentVal = currVal,
                    InterestVal = period > 0 ? interestRate * currVal : 0,
                    TotalVal = initVal * Math.Pow((1 + interestRate), period)
                };
                currVal = row.TotalVal;
                retVal.Add(row);
            }
            return retVal;
        }
    }
}