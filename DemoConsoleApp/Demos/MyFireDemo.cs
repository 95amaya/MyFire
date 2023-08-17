using AutoMapper;
using Google.Apis.Sheets.v4;
using DemoConsoleApp.Models;
using CoreLibraries;
using System;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Dapper;
using System.IO;
using Services.Models;
using Dapper.Contrib.Extensions;

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
        // DapperExtensions.DapperExtensions.SqlDialect = new DapperExtensions.Sql.MySqlDialect();

        // Get From Sheet
        // var googleSheetApiClient = Helper.InitializeSheetService(ApplicationName, Scopes);
        // var googleSheetReader = new GoogleSheetReader(_mapper, new GoogleSheetClient(googleSheetApiClient));
        // var billTransactions = GetBillTransactions(secrets.BillTransactionSheets.FirstOrDefault(), googleSheetReader);

        // Build output File Path
        var billTransactionsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "DemoConsoleApp", "output", "output.json");

        // write to file
        // Helper.WriteToJson(billTransactionsFilePath, billTransactions);

        // read from file
        var billTransactions = Helper.ReadFromJson<List<BillTransactionDto>>(billTransactionsFilePath);
        Console.WriteLine($"Total Bill Transactions Read: {billTransactions.Count()}");

        // Save Transactions to DB
        // https://medium.com/dapper-net/custom-columns-mapping-1cd45dfd51d6
        var connectionString = "Server=127.0.0.1;port=3306;Uid=root;Password=test_pass;Database=localdb";
        var sql = "SELECT CURDATE();";
        IEnumerable<BillTransactionDbo> billTransactionDbos;

        billTransactionDbos = _mapper.Map<List<BillTransactionDbo>>(billTransactions);
        var testList = _mapper.Map<List<BillTransactionDto>>(billTransactionDbos);

        // var billTransactionDbo = _mapper.Map<BillTransactionDbo>(billTransactions.FirstOrDefault());
        // var billTransactionDbos = _mapper.Map<List<BillTransactionDbo>>(billTransactions);

        // var test1 = billTransactions.Where(p => p.TransactionDate == null).ToList();
        // var test2 = billTransactionDbos.Where(p => p.transaction_date == null).ToList();
        // dbconnection manager
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var retVal = connection.QueryFirstOrDefault<DateTime>(sql);
            Console.WriteLine($"Mysql Connection Test Output: {retVal}");

            // bulk insert into DB
            var count = connection.Insert(billTransactionDbos);
            Console.WriteLine($"Total Inserts: {count}");
            connection.Close();

            // read from DB
            // billTransactionDbos = connection.GetAll<BillTransactionDbo>();

            // billTransactionDbos = connection.Query<BillTransactionDbo>(@"
            //     select * from BillTransactionDbo
            //     where
            //         transaction_type = @foo
            // ;", new { foo = TransactionType.DEBIT.ToString() });

            // Console.WriteLine($"Total Bill Transactions Read: {billTransactionDbos.Count()}");
        }

        // var mapTest = _mapper.Map<List<BillTransactionDto>>(billTransactionDbos);
        Console.WriteLine("successfully mapped!!!");
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

        Console.WriteLine($"Total Bill Transactions Written: {transactionList.Count()}");

        return transactionList;
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