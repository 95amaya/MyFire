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