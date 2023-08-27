using AutoMapper;
using Google.Apis.Sheets.v4;
using DemoConsoleApp.Models;
using CoreLibraries;
using System;
using System.Linq;
using System.Collections.Generic;
using Dapper;
using Services.Models;
using Services.CoreLibraries;
using Services;

namespace DemoConsoleApp.Demos;
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
        SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);

        var connManager = new MySqlDbConnectionManager(secrets.ConnectionString);
        var daoDb = new BillTransactionDaoDb(connManager, _mapper);
        var transactionDtos = daoDb.Get(new DateTime(2023, 1, 1));

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

        var test = transactionDtos.Count();
    }

    private static long BulkInsertFromSheet(Secrets secrets, IMapper _mapper)
    {
        var googleSheetApiClient = Helper.InitializeSheetService(ApplicationName, Scopes);
        var googleSheetReader = new GoogleSheetReader(_mapper, new GoogleSheetClient(googleSheetApiClient));
        var billTransactionDtos = GetBillTransactions(secrets.BillTransactionSheets.FirstOrDefault(), googleSheetReader);

        // dbconnection manager
        var connManager = new MySqlDbConnectionManager(secrets.ConnectionString);
        var daoDb = new BillTransactionDaoDb(connManager, _mapper);

        return daoDb.BulkInsert(billTransactionDtos);
    }

    private static List<BillTransactionDto> GetBillTransactions(BillTransactionSheet transactionSheet, GoogleSheetReader googleSheetReader)
    {
        var transactionList = new List<BillTransactionDto>();

        // read from google sheet
        var needsCheckingTransactions = googleSheetReader.ReadFrom<WfNeedsCheckingBillTransactionDto>(transactionSheet.SheetId, transactionSheet.NeedsCheckingTransactionRange);
        var wantsCheckingTransactions = googleSheetReader.ReadFrom<WfWantsCheckingBillTransactionDto>(transactionSheet.SheetId, transactionSheet.WantsCheckingTransactionRange);
        var needsCardTransactions = googleSheetReader.ReadFrom<WfNeedsCardBillTransactionDto>(transactionSheet.SheetId, transactionSheet.NeedsCardTransactionRange);
        var wantsCardTransactions = googleSheetReader.ReadFrom<JpmWantsCardBillTransactionDto>(transactionSheet.SheetId, transactionSheet.WantsCardTransactionRange);

        // Prints my transactions from spreadsheet
        PrintSampleOfDataSet("NEEDS CHECKING Sample", needsCheckingTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("WANTS CHECKING Sample", wantsCheckingTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("NEEDS CARD Sample", needsCardTransactions.Cast<BillTransactionDto>());
        PrintSampleOfDataSet("WANTS CARD Sample", wantsCardTransactions.Cast<BillTransactionDto>());

        transactionList.AddRange(needsCheckingTransactions);
        transactionList.AddRange(wantsCheckingTransactions);
        transactionList.AddRange(needsCardTransactions);
        transactionList.AddRange(wantsCardTransactions);

        Console.WriteLine($"Total Bill Transactions Read: {transactionList.Count()}");

        return transactionList;
    }

    private static bool TestDbConnection(IDbConnectionManager dbConnectionManager)
    {
        var sql = "SELECT CURDATE();";

        using var connection = dbConnectionManager.CreateConnection();
        var retVal = connection.QueryFirstOrDefault<DateTime>(sql);
        Console.WriteLine($"Mysql Connection Test Output: {retVal}");
        return true;
    }
    private static IMapper InitializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<IList<object>, WfBillTransactionDto>()
                .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src[0]))
                .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[1]))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src[4]));

            cfg.CreateMap<IList<object>, WfNeedsCheckingBillTransactionDto>()
                .IncludeBase<IList<object>, WfBillTransactionDto>();
            cfg.CreateMap<IList<object>, WfWantsCheckingBillTransactionDto>()
                .IncludeBase<IList<object>, WfBillTransactionDto>();
            cfg.CreateMap<IList<object>, WfNeedsCardBillTransactionDto>()
                .IncludeBase<IList<object>, WfBillTransactionDto>();

            cfg.CreateMap<IList<object>, JpmBillTransactionDto>()
                .ForMember(dest => dest.TransactionDate, act => act.MapFrom(src => src[0]))
                .ForMember(dest => dest.Amount, act => act.MapFrom(src => src[5]))
                .ForMember(dest => dest.Description, act => act.MapFrom(src => src[2]));

            cfg.CreateMap<IList<object>, JpmWantsCardBillTransactionDto>()
                .IncludeBase<IList<object>, JpmBillTransactionDto>();

            cfg.CreateMap<BillTransactionDto, BillTransactionDbo>()
                .ForMember(dest => dest.id, act => act.MapFrom(src => src.Id))
                .ForMember(dest => dest.transaction_date, act => act.MapFrom(src => src.TransactionDate))
                .ForMember(dest => dest.amount, act => act.MapFrom(src => src.Amount))
                .ForMember(dest => dest.description, act => act.MapFrom(src => src.Description))
                .ForMember(dest => dest.transaction_type, act => act.Ignore())
                .ForMember(dest => dest.transaction_account, act => act.Ignore())
                .ForMember(dest => dest.transaction_type, act => act.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.transaction_account, act => act.MapFrom(src => src.Account.ToString()))
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