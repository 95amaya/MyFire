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
using MyFire.Services;
using MyFire.Models;

namespace SheetsQuickstart
{
    public static class GoogleSheetsServicesDemo
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
            var googleSheetApiClient = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            // Create Reader 
            var googleSheetReader = new GoogleSheetReader(_mapper, new GoogleSheetClient(googleSheetApiClient)); 

            // Define request parameters.
            String spreadsheetId = "1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms";
            String range = "Class Data!A2:E";

            var students = googleSheetReader.ReadFrom<Student>(spreadsheetId, range); 

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            foreach(var student in students)
            {
                Console.WriteLine(student);
            }
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
}