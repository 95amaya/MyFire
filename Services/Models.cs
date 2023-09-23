using Services.CoreLibraries;

namespace Services.Models;

public class BillTransactionSheet
{
    public string SheetId { get; set; } = string.Empty;
    public string NeedsDebitTransactionRange { get; set; } = string.Empty;
    public string WantsDebitTransactionRange { get; set; } = string.Empty;
    public string NeedsCreditTransactionRange { get; set; } = string.Empty;
    public string WantsCreditTransactionRange { get; set; } = string.Empty;
}

public enum TransactionType
{
    DEBIT = 0,
    CREDIT = 1,
}

public enum TransactionAccount
{
    NEEDS = 0,
    WANTS = 1,
}

// TODO: Update This now that it doesn't need to be DB compliant
public class BillTransactionCsv : ICsvRecord
{
    public DateTime? transaction_date { get; set; }
    public decimal? amount { get; set; }
    public string? description { get; set; }
    public string? transaction_type { get; set; }
    public string? transaction_account { get; set; }
    public bool is_noise { get; set; }

    public string GetCsvHeader() => $"Transaction Date,Amount,Transaction Type,Account,Description,Is Noise";

    public string GetCsvRow() => $"{transaction_date?.Date.ToString("""yyyy-MM-dd""")},{amount},{transaction_type},{transaction_account},\"{description}\",{is_noise}";
}

public class BillTransactionDto
{
    public DateTime? TransactionDate { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionAccount? Account { get; set; }
    public bool IsNoise { get; set; }

    public BillTransactionDto() { }

    public BillTransactionDto(TransactionType type, TransactionAccount account)
    {
        Type = type;
        Account = account;
    }

    public override string ToString()
    {
        return $"{Account}, {Type}, {TransactionDate?.ToString("""MM/dd/yyyy""")}, {Amount}, {Description}";
    }
}

public class WfBillTransactionDto : BillTransactionDto
{
    public WfBillTransactionDto() { }
    public WfBillTransactionDto(TransactionType type, TransactionAccount account) : base(type, account) { }
}
public class JpmBillTransactionDto : BillTransactionDto
{
    public JpmBillTransactionDto() { }
    public JpmBillTransactionDto(TransactionType type, TransactionAccount account) : base(type, account) { }
}

public class WfNeedsDebitBillTransactionDto : WfBillTransactionDto
{
    public WfNeedsDebitBillTransactionDto() : base(TransactionType.DEBIT, TransactionAccount.NEEDS) { }
}
public class WfNeedsCreditBillTransactionDto : WfBillTransactionDto
{
    public WfNeedsCreditBillTransactionDto() : base(TransactionType.CREDIT, TransactionAccount.NEEDS) { }
}
public class WfWantsDebitBillTransactionDto : WfBillTransactionDto
{
    public WfWantsDebitBillTransactionDto() : base(TransactionType.DEBIT, TransactionAccount.WANTS) { }
}
public class JpmWantsCreditBillTransactionDto : JpmBillTransactionDto
{
    public JpmWantsCreditBillTransactionDto() : base(TransactionType.CREDIT, TransactionAccount.WANTS) { }
}
