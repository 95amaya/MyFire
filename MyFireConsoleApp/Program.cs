using Demo;
using Google.Apis.Sheets.v4;
using System;

namespace MyFireConsoleApp;
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        // GoogleSheetsDemo.Run(args);
        GoogleSheetsServicesDemo.Run(args);
        
        // -- Create Google Sheet 
        // var googleSheetsApiService = NewSheetService();
        // var financeService = new FinanceService();
        // var googleSheetsService = new GoogleSheetReader(googleSheetsApiService, ALPHA);
        // var testData = googleSheetsService.ReadSheet<Test>(SHEET_ID, "A1:B2");
        // var cmpIntTable = financeService.BuildCompoundInterestTable(1000, .15, 5);
        // var resp = googleSheetsService.WriteSheet(cmpIntTable, SHEET_ID, "A1:D20", header:"Year,Curr,Intr,Total,rate=5%,contribution=");
        // Console.WriteLine(JsonConvert.SerializeObject(resp));
    }

    
}
