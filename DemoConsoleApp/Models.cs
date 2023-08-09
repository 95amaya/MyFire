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
}

public enum TransactionCategory
{
    NEEDS_CHECKING = 0,
    WANTS_CHECKING = 1,
    NEEDS_CARD = 2,
    WANTS_CARD = 3,
}

public class BillTransaction
{
    public DateTime TransactionDate { get; set; }
    public double Amount { get; set; }
    public string Description { get; set; }

    public override string ToString()
    {
        return TransactionDate.ToString("MM/dd/yyyy") + $", {Amount}, {Description}";
    }
}

public class WfBillTransaction : BillTransaction { }
public class JpmBillTransaction : BillTransaction { }