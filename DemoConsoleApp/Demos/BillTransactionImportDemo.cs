using AutoMapper;
using CoreLibraries;
using DemoConsoleApp.Mappings;
using DemoConsoleApp.Models;
using Google.Apis.Sheets.v4;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoConsoleApp.Demos;

public static class BillTransactionImportDemo
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/credentials.json
    // spreadsheet is defined locally by creating a ~/secrets.json file and storing a "SheetId" to retrieve
    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static readonly string ApplicationName = "MyFire Google Sheets API Demo";

    public static void Run(string[] args, Secrets secrets)
    {
        var _mapper = InitializeAutomapper();
        ImportBillTransactionsFromSheet(_mapper, secrets.ImportSheet);

    }

    // FIX: Will Break due to mapping changes
    private static List<BillTransactionCsvo> ImportBillTransactionsFromSheet(IMapper _mapper, BillTransactionSheet transactionSheet)
    {
        var transactionList = new List<BillTransactionCsvo>();

        var googleSheetApiClient = Helper.InitializeSheetService(ApplicationName, Scopes);
        var googleSheetReader = new GoogleSheetReader(_mapper, new GoogleSheetClient(googleSheetApiClient));
        var needsDebitTransactions = googleSheetReader.Read<WfNeedsDebitBillTransactionCsvo>(transactionSheet.SheetId, transactionSheet.NeedsDebitTransactionRange);
        var wantsDebitTransactions = googleSheetReader.Read<WfWantsDebitBillTransactionCsvo>(transactionSheet.SheetId, transactionSheet.WantsDebitTransactionRange);
        var needsCreditTransactions = googleSheetReader.Read<WfNeedsCreditBillTransactionCsvo>(transactionSheet.SheetId, transactionSheet.NeedsCreditTransactionRange);
        var wantsCreditTransactions = googleSheetReader.Read<JpmWantsCreditBillTransactionCsvo>(transactionSheet.SheetId, transactionSheet.WantsCreditTransactionRange);

        // Prints sample transactions
        PrintSampleOfDataSet("NEEDS DEBIT Sample", needsDebitTransactions.Cast<BillTransactionCsvo>());
        PrintSampleOfDataSet("WANTS DEBIT Sample", wantsDebitTransactions.Cast<BillTransactionCsvo>());
        PrintSampleOfDataSet("NEEDS CREDIT Sample", needsCreditTransactions.Cast<BillTransactionCsvo>());
        PrintSampleOfDataSet("WANTS CREDIT Sample", wantsCreditTransactions.Cast<BillTransactionCsvo>());

        transactionList.AddRange(needsDebitTransactions);
        transactionList.AddRange(wantsDebitTransactions);
        transactionList.AddRange(needsCreditTransactions);
        transactionList.AddRange(wantsCreditTransactions);
        Console.WriteLine($"Total Bill Transactions Read: {transactionList.Count()}");

        return transactionList;
    }

    private static IMapper InitializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ImportMapProfile>();
            cfg.AddProfile<ExportMapProfile>();
            cfg.AddProfile<TransformMapProfile>();
        });
        return config.CreateMapper();
    }

    private static void PrintSampleOfDataSet(string title, IEnumerable<BillTransactionCsvo> transactions)
    {
        Console.WriteLine($"{title}, Count: {transactions.Count()}");
        foreach (var transaction in transactions.Take(5))
        {
            Console.WriteLine(transaction);
        }
        Console.WriteLine();
    }

    private static void PrintSampleOfDataSet(string title, IEnumerable<BillTransactionDto> transactions)
    {
        Console.WriteLine($"{title}, Count: {transactions.Count()}");
        foreach (var transaction in transactions.Take(5))
        {
            Console.WriteLine(transaction);
        }
        Console.WriteLine();
    }
}