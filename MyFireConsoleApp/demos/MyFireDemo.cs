using AutoMapper;
using Google.Apis.Sheets.v4;
using MyFireConsoleApp.Models;
using MyFireCoreLibraries;
using System;
using System.Linq;
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
        var transactionList = new List<BillTransaction>();
        var _mapper = InitializeAutomapper();

        // Create Google Sheets API service.
        var googleSheetApiClient = DemoHelper.InitializeSheetService(ApplicationName, Scopes);

        // Create Reader 
        var googleSheetReader = new GoogleSheetReader(_mapper, new GoogleSheetClient(googleSheetApiClient));

        var needsCheckingTransactions = googleSheetReader.ReadFrom<WfBillTransaction>(secrets.SheetId, secrets.NeedsCheckingTransactionRange);
        var wantsCheckingTransactions = googleSheetReader.ReadFrom<WfBillTransaction>(secrets.SheetId, secrets.WantsCheckingTransactionRange);
        var needsCardTransactions = googleSheetReader.ReadFrom<WfBillTransaction>(secrets.SheetId, secrets.NeedsCardTransactionRange);
        var wantsCardTransactions = googleSheetReader.ReadFrom<JpmBillTransaction>(secrets.SheetId, secrets.WantsCardTransactionRange);

        // Prints my transactions from spreadsheet
        printSampleOfDataSet("NEEDS CHECKING Sample", needsCheckingTransactions.Cast<BillTransaction>());
        printSampleOfDataSet("WANTS CHECKING Sample", wantsCheckingTransactions.Cast<BillTransaction>());
        printSampleOfDataSet("NEEDS CARD Sample", needsCardTransactions.Cast<BillTransaction>());
        printSampleOfDataSet("WANTS CARD Sample", wantsCardTransactions.Cast<BillTransaction>());

        transactionList.AddRange(needsCardTransactions);
        transactionList.AddRange(wantsCheckingTransactions);
        transactionList.AddRange(needsCardTransactions);
        transactionList.AddRange(wantsCardTransactions);

        Console.WriteLine($"Total Bill Transactions: {transactionList.Count()}");
    }

    private static IMapper InitializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IList<Object>, WfBillTransaction>()
                .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src[0]))
                .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[1]))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src[4]));

            cfg.CreateMap<IList<Object>, JpmBillTransaction>()
                .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src[0]))
                .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[5]))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src[2]));
        });
        return config.CreateMapper();
    }

    private static void printSampleOfDataSet(string title, IEnumerable<BillTransaction> transactions)
    {
        Console.WriteLine(title);
        foreach (var transaction in transactions.Take(5))
        {
            Console.WriteLine(transaction);
        }
        Console.WriteLine();
    }
}