using System.Collections.Generic;
using System.IO;
using System.Linq;
using Services.Models;

namespace DemoConsoleApp.Models;

public class Secrets
{
    public BillTransactionSheet ImportSheet { get; set; }
}

public class BillTransactionSheet
{
    public string SheetId { get; set; } = string.Empty;
    public string NeedsDebitTransactionRange { get; set; } = string.Empty;
    public string WantsDebitTransactionRange { get; set; } = string.Empty;
    public string NeedsCreditTransactionRange { get; set; } = string.Empty;
    public string WantsCreditTransactionRange { get; set; } = string.Empty;
}