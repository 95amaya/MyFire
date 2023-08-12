using System;

namespace MyFireConsoleApp.Models;

public class Student
{
    public string FirstName { get; set; }
    public string Sex { get; set; }
    public string Class { get; set; }
    public string City { get; set; }
    public string Major { get; set; }

    public override string ToString()
    {
        return $"{FirstName}, {Major}, {City}";
    }
}

public class Secrets
{
    public string SheetId { get; set; }
    public string NeedsCheckingTransactionRange { get; set; }
    public string WantsCheckingTransactionRange { get; set; }
    public string NeedsCardTransactionRange { get; set; }
    public string WantsCardTransactionRange { get; set; }
    public string FileWritePath { get; set; }
}

public enum TransactionCategory
{
    NEEDS_CHECKING = 0,
    WANTS_CHECKING = 1,
    NEEDS_CARD = 2,
    WANTS_CARD = 3,
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

public class BillTransaction
{
    public DateTime TransactionDate { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }
    public TransactionCategory? Category { get; set; }
    public TransactionType? Type { get; set; }
    public TransactionAccount? Account { get; set; }

    public BillTransaction() { }

    public BillTransaction(TransactionCategory category)
    {
        Category = category;
    }

    public BillTransaction(TransactionType type, TransactionAccount account)
    {
        Type = type;
        Account = account;
    }

    public override string ToString()
    {
        return $"{Account}, {Type}, {TransactionDate.ToString("""MM/dd/yyyy""")}, {Amount}, {Description}";
    }
}

public class WfBillTransaction : BillTransaction
{
    public WfBillTransaction() { }
    public WfBillTransaction(TransactionType type, TransactionAccount account) : base(type, account) { }
}
public class JpmBillTransaction : BillTransaction
{
    public JpmBillTransaction() { }
    public JpmBillTransaction(TransactionType type, TransactionAccount account) : base(type, account) { }
}

public class WfNeedsCheckingBillTransaction : WfBillTransaction
{
    public WfNeedsCheckingBillTransaction() : base(TransactionType.DEBIT, TransactionAccount.NEEDS) { }
}
public class WfNeedsCardBillTransaction : WfBillTransaction
{
    public WfNeedsCardBillTransaction() : base(TransactionType.CREDIT, TransactionAccount.NEEDS) { }
}
public class WfWantsCheckingBillTransaction : WfBillTransaction
{
    public WfWantsCheckingBillTransaction() : base(TransactionType.DEBIT, TransactionAccount.WANTS) { }
}
public class JpmWantsCardBillTransaction : JpmBillTransaction
{
    public JpmWantsCardBillTransaction() : base(TransactionType.CREDIT, TransactionAccount.WANTS) { }
}
