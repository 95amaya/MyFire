using AutoMapper;
using CoreLibraries;
using EtlJobApp.Mappings;
using EtlJobApp.Models;
using Serilog;
using Services.CoreLibraries;
using Services.Models;
using System.Text.RegularExpressions;

namespace EtlJobApp;

public static class BillTransactionEtlJob
{
    private static ILogger? Logger { get; set; }
    public static void Run(ILogger logger, Secrets secrets)
    {
        Logger = logger;
        var _mapper = InitializeAutomapper();

        // Extract
        var csvReader = new CsvReader(_mapper);
        // TODO: Can improve by including a line starts with date string in order to not read in entire file
        var exportFilePath = secrets.ExportFiles?.File2023Path ?? string.Empty;
        var exportCsvoList = GetBillTransactionsFromCsv<BillTransactionExportCsvo>(csvReader, exportFilePath, skipFirstRow: true);
        var importCsvoList = ImportBillTransactionsFromCsv(csvReader, secrets.ImportFiles);

        // Transform
        // Normalize data to compare accurately
        var cutoffDate = new DateTime(2023, 9, 25);
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
                if (item.Description != null && Regex.IsMatch(item.Description, noiseFilter))
                {
                    item.CustomTags.Add(KnownCustomTags.NOISE.ToString());
                    break;
                }
            }
        };
        PrintSampleOfDataSet("Noise List Sample", itemsToInsert.Where(p => p.CustomTags.Contains(KnownCustomTags.NOISE.ToString())).ToList());

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
        Logger?.Information($"Total Bill Transactions Read: {transactionList.Count()}");

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
        Logger?.Information($"{title}, Count: {transactions.Count()}");
        foreach (var transaction in transactions.Take(5))
        {
            Logger?.Information($"Transaction: {transaction}");
        }
    }

    private static void PrintSampleOfDataSet(string title, IEnumerable<BillTransactionDto> transactions)
    {
        Logger?.Information($"{title}, Count: {transactions.Count()}");
        foreach (var transaction in transactions.Take(5))
        {
            Logger?.Information($"Transaction: {transaction}");
        }
    }
}