using AutoMapper;
using Google.Apis.Sheets.v4;
using MyFireConsoleApp.Models;
using MyFireCoreLibraries;
using System;
using System.Collections.Generic;

namespace Demo;
public static class GoogleSheetsServicesDemo
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/.credentials/sheets.googleapis.com-dotnet-quickstart.json
    static string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static string ApplicationName = "Google Sheets API .NET Quickstart";

    public static void Run(string[] args)
    {
        var _mapper = InitializeAutomapper();

        // Create Google Sheets API service.
        var googleSheetApiClient = DemoHelper.InitializeSheetService(ApplicationName, Scopes);

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