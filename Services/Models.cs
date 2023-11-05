using System.Diagnostics.CodeAnalysis;
using Services.CoreLibraries;

namespace Services.Models;

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

public class BillTransactionDto
{
    public DateTime? TransactionDate { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionAccount? Account { get; set; }
    public bool IsNoise { get; set; }

    public override string ToString()
    {
        return $"{Account}, {Type}, {TransactionDate?.ToString("""MM/dd/yyyy""")}, {Amount}, {Description}";
    }
}

public class UniqueBillTransactionDto : EqualityComparer<BillTransactionDto>
{
    public override bool Equals(BillTransactionDto? x, BillTransactionDto? y)
    {
        if (x == null && y == null)
            return true;
        else if (x == null || y == null)
            return false;

        var hasSameDescription = (x.Description == null && y.Description == null)
        || (x.Description != null && y.Description != null && x.Description.Equals(y.Description));

        return
            x.TransactionDate == y.TransactionDate &&
            x.Amount == y.Amount &&
            hasSameDescription;
    }

    public override int GetHashCode([DisallowNull] BillTransactionDto obj)
    {
        return (obj.TransactionDate.GetHashCode() ^ obj.Amount.GetHashCode() ^ obj.Description.GetHashCode()).GetHashCode();
    }
}

public class BillTransactionCsvo : ICsvRecord
{
    public string TransactionDate { get; set; } = string.Empty;
    public string Amount { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string IsNoise { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{Account}, {Type}, {TransactionDate}, {Amount}, {Description}";
    }

    public string GetCsvHeader() => $"Transaction Date,Amount,Transaction Type,Account,Description,Is Noise"; // Tags

    public string GetCsvRow() => $"{TransactionDate},{Amount},{Type},{Account},\"{Description}\",{IsNoise}";
}

public class BillTransactionExportCsvo : BillTransactionCsvo { }
public class BillTransactionImportCsvo : BillTransactionCsvo
{
    public BillTransactionImportCsvo() { }
    public BillTransactionImportCsvo(TransactionType type, TransactionAccount account)
    {
        Type = type.ToString();
        Account = account.ToString();
    }
}

public class WfBillTransactionCsvo : BillTransactionImportCsvo
{
    public WfBillTransactionCsvo() { }
    public WfBillTransactionCsvo(TransactionType type, TransactionAccount account) : base(type, account) { }
}
public class JpmBillTransactionCsvo : BillTransactionImportCsvo
{
    public JpmBillTransactionCsvo() { }
    public JpmBillTransactionCsvo(TransactionType type, TransactionAccount account) : base(type, account) { }
}

public class WfNeedsDebitBillTransactionCsvo : WfBillTransactionCsvo
{
    public WfNeedsDebitBillTransactionCsvo() : base(TransactionType.DEBIT, TransactionAccount.NEEDS) { }
}
public class WfNeedsCreditBillTransactionCsvo : WfBillTransactionCsvo
{
    public WfNeedsCreditBillTransactionCsvo() : base(TransactionType.CREDIT, TransactionAccount.NEEDS) { }
}
public class WfWantsDebitBillTransactionCsvo : WfBillTransactionCsvo
{
    public WfWantsDebitBillTransactionCsvo() : base(TransactionType.DEBIT, TransactionAccount.WANTS) { }
}
public class JpmWantsCreditBillTransactionCsvo : JpmBillTransactionCsvo
{
    public JpmWantsCreditBillTransactionCsvo() : base(TransactionType.CREDIT, TransactionAccount.WANTS) { }
}
