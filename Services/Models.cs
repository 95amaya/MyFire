
using Dapper.Contrib.Extensions;

namespace Services.Models;

public class BillTransactionSheet
{
    public string SheetId { get; set; }
    public string NeedsCheckingTransactionRange { get; set; }
    public string WantsCheckingTransactionRange { get; set; }
    public string NeedsCardTransactionRange { get; set; }
    public string WantsCardTransactionRange { get; set; }
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

[Table("bill_transactions")]
// BillTransactionDbo
public class BillTransactionDbo
{
    [Key]
    public int? id { get; set; }
    public DateTime? transaction_date { get; set; }
    public double? amount { get; set; }
    public string description { get; set; }
    public string transaction_type { get; set; }
    public string transaction_account { get; set; }

    public string GetTransactionTypeFilterStr(string propName) => $" {nameof(transaction_type)} = @{propName} ";
}

public class BillTransactionDto
{
    public int? Id { get; set; }
    public DateTime? TransactionDate { get; set; }
    public double? Amount { get; set; }
    public string Description { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionAccount? Account { get; set; }

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

public class WfNeedsCheckingBillTransactionDto : WfBillTransactionDto
{
    public WfNeedsCheckingBillTransactionDto() : base(TransactionType.DEBIT, TransactionAccount.NEEDS) { }
}
public class WfNeedsCardBillTransactionDto : WfBillTransactionDto
{
    public WfNeedsCardBillTransactionDto() : base(TransactionType.CREDIT, TransactionAccount.NEEDS) { }
}
public class WfWantsCheckingBillTransactionDto : WfBillTransactionDto
{
    public WfWantsCheckingBillTransactionDto() : base(TransactionType.DEBIT, TransactionAccount.WANTS) { }
}
public class JpmWantsCardBillTransactionDto : JpmBillTransactionDto
{
    public JpmWantsCardBillTransactionDto() : base(TransactionType.CREDIT, TransactionAccount.WANTS) { }
}
