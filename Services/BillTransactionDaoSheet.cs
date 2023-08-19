using CoreLibraries;
using Services.Models;

namespace Services;

public class BillTransactionDaoSheet : IBillTransactionDaoReader
{
    private BillTransactionSheet _sheet { get; set; }
    private ISheetReader _sheetReader { get; set; }

    public BillTransactionDaoSheet(ISheetReader sheetReader, BillTransactionSheet sheet)
    {
        _sheet = sheet;
        _sheetReader = sheetReader;
    }

    public IEnumerable<BillTransactionDto> GetList()
    {
        var transactionList = new List<BillTransactionDto>();

        // read from google sheet
        var needsCheckingTransactions = _sheetReader.ReadFrom<WfNeedsCheckingBillTransactionDto>(_sheet.SheetId, _sheet.NeedsCheckingTransactionRange);
        var wantsCheckingTransactions = _sheetReader.ReadFrom<WfWantsCheckingBillTransactionDto>(_sheet.SheetId, _sheet.WantsCheckingTransactionRange);
        var needsCardTransactions = _sheetReader.ReadFrom<WfNeedsCardBillTransactionDto>(_sheet.SheetId, _sheet.NeedsCardTransactionRange);
        var wantsCardTransactions = _sheetReader.ReadFrom<JpmWantsCardBillTransactionDto>(_sheet.SheetId, _sheet.WantsCardTransactionRange);

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
