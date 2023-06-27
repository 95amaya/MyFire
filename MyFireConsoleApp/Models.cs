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
    public const string FilePath = "Secrets.json";
    public string SheetId { get; set; }
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