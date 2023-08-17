using CoreLibraries;
using Services.Models;

namespace Services;

public class BillTransactionSheetDao : IBillTransactionSheetDao
{
    private ISheetReader _sheetReader { get; set; }

    public BillTransactionSheetDao(ISheetReader sheetReader)
    {
        _sheetReader = sheetReader;
    }

    public List<BillTransactionDto> GetTransactions(BillTransactionSheet sheet)
    {
        var transactionList = new List<BillTransactionDto>();

        // read from google sheet
        var needsCheckingTransactions = _sheetReader.ReadFrom<WfNeedsCheckingBillTransactionDto>(sheet.SheetId, sheet.NeedsCheckingTransactionRange);
        var wantsCheckingTransactions = _sheetReader.ReadFrom<WfWantsCheckingBillTransactionDto>(sheet.SheetId, sheet.WantsCheckingTransactionRange);
        var needsCardTransactions = _sheetReader.ReadFrom<WfNeedsCardBillTransactionDto>(sheet.SheetId, sheet.NeedsCardTransactionRange);
        var wantsCardTransactions = _sheetReader.ReadFrom<JpmWantsCardBillTransactionDto>(sheet.SheetId, sheet.WantsCardTransactionRange);

        // Prints my transactions from spreadsheet
        // implement logger
        // PrintSampleOfDataSet("NEEDS CHECKING Sample", needsCheckingTransactions.Cast<BillTransactionDto>());
        // PrintSampleOfDataSet("WANTS CHECKING Sample", wantsCheckingTransactions.Cast<BillTransactionDto>());
        // PrintSampleOfDataSet("NEEDS CARD Sample", needsCardTransactions.Cast<BillTransactionDto>());
        // PrintSampleOfDataSet("WANTS CARD Sample", wantsCardTransactions.Cast<BillTransactionDto>());

        transactionList.AddRange(needsCheckingTransactions);
        transactionList.AddRange(wantsCheckingTransactions);
        transactionList.AddRange(needsCardTransactions);
        transactionList.AddRange(wantsCardTransactions);

        // Console.WriteLine($"Total Bill Transactions Written: {transactionList.Count()}");

        return transactionList;
    }
}
