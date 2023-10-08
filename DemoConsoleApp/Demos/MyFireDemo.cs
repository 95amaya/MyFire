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
    public static void Run(string[] args, Secrets secrets)
    {
        var _mapper = InitializeAutomapper();

        // Run aggregation report
        // Console.WriteLine(DateTime.Now.ToString("s"));
        // Console.WriteLine(DateTime.Now.Date.ToString("s"));
        // Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));
        // Console.WriteLine(DateOnly.FromDateTime(DateTime.Now).ToString("s"));

        var csvReader = new CsvReader(_mapper);
        var exportCsvoList = GetBillTransactionsFromCsv<BillTransactionExportCsvo>(csvReader, secrets.ExportFiles.File2023Path, skipFirstRow: true);

        var startDate = new DateTime(2023, 09, 01);
        var endDate = startDate.AddMonths(1);

        var exportDtoList = _mapper.Map<IEnumerable<BillTransactionDto>>(exportCsvoList).Where(p => !p.IsNoise && p.TransactionDate >= startDate && p.TransactionDate < endDate);

        // TODO: Clean up reports
        RunReport("Needs Debit Report", exportDtoList, TransactionType.DEBIT, TransactionAccount.NEEDS);
        RunReport("Wants Debit Report", exportDtoList, TransactionType.DEBIT, TransactionAccount.WANTS);

    }

    private static void RunReport(string title, IEnumerable<BillTransactionDto> list, TransactionType type, TransactionAccount account)
    {
        Console.WriteLine();
        Console.WriteLine(title);
        var items = list.Where(p => p.Type == type && p.Account == account).ToList();

        Console.WriteLine("Income");
        var incomeList = items.Where(p => p.Amount > 0).ToList();
        incomeList.ForEach(Console.WriteLine);
        Console.WriteLine($"Total = {incomeList.Sum(p => p.Amount)}");

        Console.WriteLine();
        Console.WriteLine("Spending");
        var spendingList = items.Where(p => p.Amount < 0).ToList();
        spendingList.ForEach(Console.WriteLine);
        Console.WriteLine($"Total = {spendingList.Sum(p => p.Amount)}");
        Console.WriteLine("---------------------------------------------");

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

    private static List<T> GetBillTransactionsFromCsv<T>(CsvReader csvReader, string path, bool skipFirstRow = false) where T : class, new()
    {
        using var sr = new StreamReader(path);
        return csvReader.Read<T>(sr, skipFirstRow).ToList();
    }
}