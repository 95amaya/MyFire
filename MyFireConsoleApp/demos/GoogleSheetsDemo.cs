using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;

namespace Demo;
public static class GoogleSheetsDemo
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
    static string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static string ApplicationName = "Google Sheets API .NET Quickstart";

    public static void Run(string[] args)
    {
        // Create Google Sheets API service.
        var service = DemoHelper.InitializeSheetService(ApplicationName, Scopes);

        // Define request parameters.
        String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
        String range = "Class Data!A2:E";
        SpreadsheetsResource.ValuesResource.GetRequest request =
                service.Spreadsheets.Values.Get(spreadsheetId, range);

        // Prints the names and majors of students in a sample spreadsheet:
        // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
        ValueRange response = request.Execute();
        IList<IList<Object>> values = response.Values;

        if (values != null && values.Count > 0)
        {
            Console.WriteLine("Name, Major");
            foreach (var row in values)
            {
                // Print columns A and E, which correspond to indices 0 and 4.
                Console.WriteLine(string.Join(",", row));

            }
        }
        else
        {
            Console.WriteLine("No data found.");
        }
    }
}