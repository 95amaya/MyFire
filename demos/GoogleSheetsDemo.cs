using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using AutoMapper;

namespace SheetsQuickstart
{
    public static class GoogleSheetsDemo
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
        static string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static string ApplicationName = "Google Sheets API .NET Quickstart";

        public static void Run(string[] args)
        {
            var _mapper = InitializeAutomapper();

            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Define request parameters.
            String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
            String range = "Class Data!A2:E";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;

            var students = _mapper.Map<IList<Student>>(values);

            // if (values != null && values.Count > 0)
            // {
            //     Console.WriteLine("Name, Major");
            //     foreach (var row in values)
            //     {
            //         // Print columns A and E, which correspond to indices 0 and 4.
            //         // Console.WriteLine(string.Join(",", row));
            //         Console.WriteLine($"{row[0]} {row[3]}");

            //     }
            // }
            // else
            // {
            //     Console.WriteLine("No data found.");
            // }
        }
    
        private static IMapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<IList<Object>, Student>()
                    .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src[0]))
                    .ForMember(dest => dest.Sex, act => act.MapFrom(src => src[1]))
                    .ForMember(dest => dest.Class, act => act.MapFrom(src => src[2]))
                    .ForMember(dest => dest.City, act => act.MapFrom(src => src[3]))
                    .ForMember(dest => dest.Major, act => act.MapFrom(src => src[4]));
            });
            return config.CreateMapper();
        }
    }

    public class Student
    {
        public string FirstName {get; set;}
        public string Sex {get; set;}
        public string Class {get; set;}
        public string City {get; set;}
        public string Major {get; set;}
    }
}