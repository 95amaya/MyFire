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

namespace DemoConsoleApp.Demos;
// TODO: use serilog for logging
// TODO: Get rid of SQL in favor of reading and writing to CSV Files (Need to take into account unique constraint)
// TODO: Perform Regex on rest of SQL Data
// -- What format makes sense here? (something easy to read in, csv) 
// TODO: Copy whole / part of DB to Google Drive Sync Folder to preserve data (chunk by year)

public static class MyFireDemo
{
    // If modifying these scopes, delete your previously saved credentials
    // at ~/credentials.json
    // spreadsheet is defined locally by creating a ~/secrets.json file and storing a "SheetId" to retrieve
    static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
    static readonly string ApplicationName = "MyFire Google Sheets API Demo";

    public static void Run(string[] args, Secrets secrets)
    {
        var _mapper = InitializeAutomapper();

        // ---- BEGIN Run BillTransactions ETL ----
        // DateTime.TryParseExact("08/31/2023", "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var testConvert1);
        // DateTime.TryParseExact("\"08/31/2023\"".Replace("\"", ""), "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var testConvert2);

        // Extract
        var billTransactionDtos = GetBillTransactionsFromSheet(_mapper, secrets.ImportSheet);
        // var billTransactionDtos = GetBillTransactionsFromCsv(_mapper, secrets.ImportFiles, secrets.BillTransactionNoiseFilterList);

        // Transform
        foreach (var item in billTransactionDtos)
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
        PrintSampleOfDataSet("Noise List Sample", billTransactionDtos.Where(p => p.IsNoise).ToList());

        var billtransactionCsvs = _mapper.Map<IEnumerable<BillTransactionCsv>>(billTransactionDtos.OrderBy(p => p.TransactionDate));

        // Load
        // // csv writer
        var csvWriter = new CsvWriter();

        var cnt = csvWriter.Write(Path.Combine(secrets.ExportFiles.DirPath, "2020-BillTransactions.csv"), billtransactionCsvs);
        Console.WriteLine($"{cnt} Transactions Written");
        // ---- END Run BillTransactions ETL ----

        // Run aggregation report
        // Console.WriteLine(DateTime.Now.ToString("s"));
        // Console.WriteLine(DateTime.Now.Date.ToString("s"));
        // Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd"));
        // Console.WriteLine(DateOnly.FromDateTime(DateTime.Now).ToString("s"));
        RunReport(_mapper, secrets.ExportFiles.DirPath, new DateTime(2022, 12, 01));

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

    private static List<BillTransactionDto> GetBillTransactionsFromCsv(IMapper _mapper, BillTransactionImport import)
    {
        var transactionList = new List<BillTransactionDto>();

        var csvReader = new CsvReader(_mapper);
        var needsDebitTransactions = csvReader.Read<WfNeedsDebitBillTransactionDto>(import.NeedsDebitPath);
        var wantsDebitTransactions = csvReader.Read<WfWantsDebitBillTransactionDto>(import.WantsDebitPath);
        var needsCreditTransactions = csvReader.Read<WfNeedsCreditBillTransactionDto>(import.NeedsCreditPath);
        var wantsCreditTransactions = csvReader.Read<JpmWantsCreditBillTransactionDto>(import.WantsCreditPath, skipFirstRow: true);

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
            cfg.CreateMap<IList<object>, WfBillTransactionDto>()
                .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => DateTime.ParseExact(src[0] as string, "MM/dd/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[1]))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src[4]));


            cfg.CreateMap<IList<object>, WfNeedsDebitBillTransactionDto>()
                .IncludeBase<IList<object>, WfBillTransactionDto>();
            cfg.CreateMap<IList<object>, WfWantsDebitBillTransactionDto>()
                .IncludeBase<IList<object>, WfBillTransactionDto>();
            cfg.CreateMap<IList<object>, WfNeedsCreditBillTransactionDto>()
                .IncludeBase<IList<object>, WfBillTransactionDto>();

            cfg.CreateMap<IList<object>, JpmBillTransactionDto>()
                .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => DateTime.ParseExact(src[0] as string, "MM/dd/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[5]))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src[2]));

            cfg.CreateMap<IList<object>, JpmWantsCreditBillTransactionDto>()
                .IncludeBase<IList<object>, JpmBillTransactionDto>();

            cfg.CreateMap<BillTransactionDto, BillTransactionCsv>()
                .ForMember(dest => dest.transaction_date, act => act.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.amount, act => act.MapFrom(src => src.Amount))
                .ForMember(dest => dest.description, act => act.MapFrom(src => src.Description))
                .ForMember(dest => dest.transaction_type, act => act.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.transaction_account, act => act.MapFrom(src => src.Account.ToString()))
                .ForMember(dest => dest.is_noise, act => act.MapFrom(src => src.IsNoise))
                .ReverseMap()
                .ForPath(dest => dest.Type, act => act.MapFrom(src => Enum.Parse<TransactionType>(src.transaction_type)))
                .ForPath(dest => dest.Account, act => act.MapFrom(src => Enum.Parse<TransactionAccount>(src.transaction_account)));
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