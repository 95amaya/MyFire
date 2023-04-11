using System;
using SheetsQuickstart;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace MyFire
{
    class Program
    {
        private static readonly string APP_NAME = "MyFireApp";
        private static readonly string[] SCOPES = { SheetsService.Scope.Spreadsheets };
        private static readonly string SHEET_ID = "11whvaSd_jqHFKNy_a1yoqVFA3KVvm_XslT27eiR0Vsk";
        private static readonly char[] ALPHA =  "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            GoogleSheetsDemo.Run(args);
            // -- Create Google Sheet 
            // var googleSheetsApiService = NewSheetService();
            // var financeService = new FinanceService();
            // var googleSheetsService = new GoogleSheetsService(googleSheetsApiService, ALPHA);
            // var testData = googleSheetsService.ReadSheet<Test>(SHEET_ID, "A1:B2");
            // var cmpIntTable = financeService.BuildCompoundInterestTable(1000, .15, 5);
            // var resp = googleSheetsService.WriteSheet(cmpIntTable, SHEET_ID, "A1:D20", header:"Year,Curr,Intr,Total,rate=5%,contribution=");
            // Console.WriteLine(JsonConvert.SerializeObject(resp));
        }

        private static SheetsService NewSheetService()
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    SCOPES,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            return new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APP_NAME,
            });
        }
    }
}
