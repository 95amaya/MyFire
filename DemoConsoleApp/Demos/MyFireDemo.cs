using AutoMapper;
using Google.Apis.Sheets.v4;
using DemoConsoleApp.Models;
using CoreLibraries;
using System;
using System.Linq;
using System.Collections.Generic;
using Services.Models;
using Services.CoreLibraries;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using DemoConsoleApp.Mappings;
using System.IO.Compression;

namespace DemoConsoleApp.Demos;
// TODO: use serilog for logging
// TODO: Build Report to verify Data, what format to use here?

public static class MyFireDemo
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/credentials.json
    // spreadsheet is defined locally by creating a ~/secrets.json file and storing a "SheetId" to retrieve
    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static readonly string ApplicationName = "MyFire Google Sheets API Demo";

    public static void Run(string[] args, Secrets secrets)
    {
        var _importMapper = InitializeAutomapper();

        // ---- BEGIN Run BillTransactions ETL ----
        // DateTime.TryParseExact("08/31/2023", "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var testConvert1);
        // DateTime.TryParseExact("\"08/31/2023\"".Replace("\"", ""), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var testConvert2);

        // Extract
        var importCsvReader = new CsvReader(_importMapper);
        var srcCsvs = GetBillTransactionsFromCsv<BillTransactionCsv>(importCsvReader, secrets.PastFiles.File2023Path, skipFirstRow: true);
        var billTransactionDtos = ImportBillTransactionsFromCsv(importCsvReader, secrets.ImportFiles);
        var billtransactionCsvs = _importMapper.Map<IEnumerable<BillTransactionCsv>>(billTransactionDtos.OrderBy(p => p.TransactionDate));


        // Transform
        var itemsToInsert = billtransactionCsvs.Except(srcCsvs, new UniqueBillTransactionCsv()).ToList();

        // foreach (var item in itemsToInsert)
        // {
        //     foreach (var noiseFilter in secrets.BillTransactionNoiseFilterList)
        //     {
        //         if (Regex.IsMatch(item.description, noiseFilter))
        //         {
        //             item.is_noise = true;
        //             break;
        //         }
        //     }
        // };
        // PrintSampleOfDataSet("Noise List Sample", itemsToInsert.Where(p => p.is_noise).ToList());

        // Load
        // var csvWriter = new CsvWriter();

        // var cnt = csvWriter.Write(Path.Combine(secrets.ExportFiles.DirPath, secrets.PastFiles.File2023), itemsToInsert);
        // Console.WriteLine($"{cnt} Transactions Written");
        // ---- END Run BillTransactions ETL ----

        // Run aggregation report
        // Console.WriteLine(DateTime.Now.ToString("s"));
        // Console.WriteLine(DateTime.Now.Date.ToString("s"));
        // Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));
        // Console.WriteLine(DateOnly.FromDateTime(DateTime.Now).ToString("s"));
        // RunReport(_exportMapper, secrets.ExportFiles.DirPath, new DateTime(2022, 12, 01));

    }

    private static void RunReport(IMapper _mapper, string exportPath, DateTime sinceInclusive)
    {
        var csvWriter = new CsvWriter();


        // var dbos = _mapper.Map<IEnumerable<BillTransactionDbo>>(transactionDtos.OrderBy(p => p.TransactionDate));

        // csvWriter.Write(Path.Combine(exportPath, "2023-BillTransactions.csv"), dbos);

        // var incomeList = transactionDtos.Where(p => p.Amount > 0 && p.Account == TransactionAccount.NEEDS).ToList();
        // incomeList.ForEach(Console.WriteLine);
        // Console.WriteLine($"income total: {incomeList.Sum(p => p.Amount.GetValueOrDefault()):C0}");

        // var transactionDtos = daoDb.Get(new DateTime(2023, 7, 31));
        // var testBool = transactionDtos.Where(p => p.IsNoise.GetValueOrDefault());

        // var oneMonthRawList = transactionDtos.Where(p => p.TransactionDate.GetValueOrDefault().Month == 1).ToList();

        // var rawList = transactionDtos
        //     .Where(p => p.Amount > 0 && p.Account == TransactionAccount.NEEDS && !p.Description.Contains("ONLINE TRANSFER") && p.TransactionDate.GetValueOrDefault().Month == 1)
        //     .ToList();

        // rawList.ForEach(Console.WriteLine);
        // Console.WriteLine(rawList.Sum(p => p.Amount.GetValueOrDefault()).ToString("C0"));

        // var reportList = transactionDtos
        //     .Where(p => p.Amount > 0 && p.Account == TransactionAccount.NEEDS && !p.Description.Contains("ONLINE TRANSFER"))
        //     .GroupBy(p => p.TransactionDate.HasValue ? p.TransactionDate.Value.Month : -1)
        //     .Select(grp => new
        //     {
        //         MonthNum = grp.Key,
        //         MonthlyTotal = grp.Sum(p => p.Amount.GetValueOrDefault()).ToString("C2"),
        //     }).ToList();

        // reportList.ForEach(Console.WriteLine);
    }

    private static List<T> GetBillTransactionsFromCsv<T>(CsvReader csvReader, string path, bool skipFirstRow = false) where T : class, new()
    {
        using var sr = new StreamReader(path);
        return csvReader.Read<T>(sr, skipFirstRow).ToList();
    }
    private static List<BillTransactionDto> ImportBillTransactionsFromCsv(CsvReader csvReader, BillTransactionImport import)
    {
        var transactionList = new List<BillTransactionDto>();

        var needsDebitTransactions = GetBillTransactionsFromCsv<WfNeedsDebitBillTransactionDto>(csvReader, import.NeedsDebitPath);
        var wantsDebitTransactions = GetBillTransactionsFromCsv<WfWantsDebitBillTransactionDto>(csvReader, import.WantsDebitPath);
        var needsCreditTransactions = GetBillTransactionsFromCsv<WfNeedsCreditBillTransactionDto>(csvReader, import.NeedsCreditPath);
        var wantsCreditTransactions = GetBillTransactionsFromCsv<JpmWantsCreditBillTransactionDto>(csvReader, import.WantsCreditPath, skipFirstRow: true);

        // Prints sample transactions
        PrintSampleOfDataSet("NEEDS DEBIT Sample", needsDebitTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("WANTS DEBIT Sample", wantsDebitTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("NEEDS CREDIT Sample", needsCreditTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("WANTS CREDIT Sample", wantsCreditTransactions.Cast<BillTransactionDto>());

        transactionList.AddRange(needsDebitTransactions);
        transactionList.AddRange(wantsDebitTransactions);
        transactionList.AddRange(needsCreditTransactions);
        transactionList.AddRange(wantsCreditTransactions);
        Console.WriteLine($"Total Bill Transactions Read: {transactionList.Count()}");


        return transactionList;
    }

    private static List<BillTransactionDto> GetBillTransactionsFromSheet(IMapper _mapper, BillTransactionSheet transactionSheet)
    {
        var transactionList = new List<BillTransactionDto>();

        var googleSheetApiClient = Helper.InitializeSheetService(ApplicationName, Scopes);
        var googleSheetReader = new GoogleSheetReader(_mapper, new GoogleSheetClient(googleSheetApiClient));
        var needsDebitTransactions = googleSheetReader.Read<WfNeedsDebitBillTransactionDto>(transactionSheet.SheetId, transactionSheet.NeedsDebitTransactionRange);
        var wantsDebitTransactions = googleSheetReader.Read<WfWantsDebitBillTransactionDto>(transactionSheet.SheetId, transactionSheet.WantsDebitTransactionRange);
        var needsCreditTransactions = googleSheetReader.Read<WfNeedsCreditBillTransactionDto>(transactionSheet.SheetId, transactionSheet.NeedsCreditTransactionRange);
        var wantsCreditTransactions = googleSheetReader.Read<JpmWantsCreditBillTransactionDto>(transactionSheet.SheetId, transactionSheet.WantsCreditTransactionRange);

        // Prints sample transactions
        PrintSampleOfDataSet("NEEDS DEBIT Sample", needsDebitTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("WANTS DEBIT Sample", wantsDebitTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("NEEDS CREDIT Sample", needsCreditTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("WANTS CREDIT Sample", wantsCreditTransactions.Cast<BillTransactionDto>());

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
            cfg.AddProfile<TransformMapProfile>();
        });
        return config.CreateMapper();
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