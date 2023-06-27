using AutoMapper;
using Google.Apis.Sheets.v4;
using MyFireConsoleApp.Models;
using MyFireCoreLibraries;
using System;
using System.Collections.Generic;

namespace Demo;
public static class MyFireDemo
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/credentials.json
    // spreadsheet is defined locally by creating a ~/secrets.json file and storing a "SheetId" to retrieve
    static string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static string ApplicationName = "MyFire Google Sheets API Demo";

    public static void Run(string[] args, Secrets secrets)
    {
        var _mapper = InitializeAutomapper();

        // Create Google Sheets API service.
        var googleSheetApiClient = DemoHelper.InitializeSheetService(ApplicationName, Scopes);

        // Create Reader 
        var googleSheetReader = new GoogleSheetReader(_mapper, new GoogleSheetClient(googleSheetApiClient));

        // Define request parameters.
        String range = "NeedsChecking!A1:E"; // spreadsheet name + !range

        var transactions = googleSheetReader.ReadFrom<BillTransaction>(secrets.SheetId, range);

        // Prints my transactions from spreadsheet
        foreach (var transaction in transactions)
        {
            Console.WriteLine(transaction);
        }
    }

    private static IMapper InitializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IList<Object>, BillTransaction>()
                .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src[0]))
                .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[1]))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src[4]));
        });
        return config.CreateMapper();
    }
}