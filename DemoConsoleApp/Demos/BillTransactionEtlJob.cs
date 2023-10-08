using AutoMapper;
using Google.Apis.Sheets.v4;
using DemoConsoleApp.Models;
using CoreLibraries;
using System;
using System.Linq;
using System.Collections.Generic;
using Services.Models;
using Services.CoreLibraries;
using System.Text.RegularExpressions;
using System.IO;
using DemoConsoleApp.Mappings;

namespace DemoConsoleApp.Demos;
// TODO: use serilog for logging

public static class BillTransactionEtlJob
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/credentials.json
    // spreadsheet is defined locally by creating a ~/secrets.json file and storing a "SheetId" to retrieve
    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static readonly string ApplicationName = "MyFire Google Sheets API Demo";

    public static void Run(string[] args, Secrets secrets)
    {
        var _mapper = InitializeAutomapper();

        // Extract
        var csvReader = new CsvReader(_mapper);
        // TODO: Can improve by including a line starts with date string in order to not read in entire file
        var exportFilePath = secrets.ExportFiles.File2023Path;
        var exportCsvoList = GetBillTransactionsFromCsv<BillTransactionExportCsvo>(csvReader, exportFilePath, skipFirstRow: true);
        var importCsvoList = ImportBillTransactionsFromCsv(csvReader, secrets.ImportFiles);

        // Transform
        // Normalize data to compare accurately
        var cutoffDate = new DateTime(2023, 8, 25);
        var exportDtoList = _mapper.Map<IEnumerable<BillTransactionDto>>(exportCsvoList).Where(p => p.TransactionDate > cutoffDate).ToList();
        var importDtoList = _mapper.Map<IEnumerable<BillTransactionDto>>(importCsvoList);

        var uniqueDto = new UniqueBillTransactionDto();
        var itemsToInsert = importDtoList;

        var itemsToIgnore = exportDtoList.Intersect(importDtoList, uniqueDto).ToList();

        if (itemsToIgnore.SafeHasRows())
        {
            PrintSampleOfDataSet("Duplicate Items Found", itemsToIgnore);
            itemsToInsert = importDtoList.Except(itemsToIgnore, uniqueDto);
        }

        foreach (var item in itemsToInsert)
        {
            foreach (var noiseFilter in secrets.BillTransactionNoiseFilterList)
            {
                if (Regex.IsMatch(item.Description, noiseFilter))
                {
                    item.IsNoise = true;
                    break;
                }
            }
        };
        PrintSampleOfDataSet("Noise List Sample", itemsToInsert.Where(p => p.IsNoise).ToList());

        var csvItemsToInsert = _mapper.Map<IEnumerable<BillTransactionExportCsvo>>(itemsToInsert.OrderBy(p => p.TransactionDate));

        // Load
        if (csvItemsToInsert.SafeHasRows())
        {
            var csvWriter = new CsvWriter();

            var cnt = csvWriter.Write(exportFilePath, csvItemsToInsert);
            Console.WriteLine($"{cnt} Transactions Written");
        }

        // If satisfied with result, then sync
    }

    private static List<T> GetBillTransactionsFromCsv<T>(CsvReader csvReader, string path, bool skipFirstRow = false) where T : class, new()
    {
        using var sr = new StreamReader(path);
        return csvReader.Read<T>(sr, skipFirstRow).ToList();
    }
    private static List<BillTransactionCsvo> ImportBillTransactionsFromCsv(CsvReader csvReader, BillTransactionImport import)
    {
        var transactionList = new List<BillTransactionCsvo>();

        var needsDebitTransactions = GetBillTransactionsFromCsv<WfNeedsDebitBillTransactionCsvo>(csvReader, import.NeedsDebitPath);
        var wantsDebitTransactions = GetBillTransactionsFromCsv<WfWantsDebitBillTransactionCsvo>(csvReader, import.WantsDebitPath);
        var needsCreditTransactions = GetBillTransactionsFromCsv<WfNeedsCreditBillTransactionCsvo>(csvReader, import.NeedsCreditPath);
        var wantsCreditTransactions = GetBillTransactionsFromCsv<JpmWantsCreditBillTransactionCsvo>(csvReader, import.WantsCreditPath, skipFirstRow: true);

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