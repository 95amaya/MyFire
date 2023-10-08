using System.Collections.Generic;
using System.IO;
using System.Linq;
using Services.Models;

namespace DemoConsoleApp.Models;

public class Secrets
{
    public BillTransactionSheet ImportSheet { get; set; }
    public List<string> BillTransactionNoiseFilterList { get; set; }
    public BillTransactionImport ImportFiles { get; set; }
    public BillTransactionExport ExportFiles { get; set; }
}

public class BillTransactionImport
{
    public IEnumerable<string> PathArr { get; set; }
    public BillTransactionFiles Files { get; set; }

    public string NeedsCreditPath => Path.Combine(PathArr.Append(Files.NeedsCredit).ToArray());
    public string WantsCreditPath => Path.Combine(PathArr.Append(Files.WantsCredit).ToArray());
    public string NeedsDebitPath => Path.Combine(PathArr.Append(Files.NeedsDebit).ToArray());
    public string WantsDebitPath => Path.Combine(PathArr.Append(Files.WantsDebit).ToArray());
}

public class BillTransactionFiles
{
    public string NeedsCredit { get; set; }
    public string WantsCredit { get; set; }
    public string NeedsDebit { get; set; }
    public string WantsDebit { get; set; }
}

public class BillTransactionExport
{
    public IEnumerable<string> PathArr { get; set; }
    public string File2023 { get; set; }

    public string DirPath => Path.Combine(PathArr.ToArray());
    public string File2023Path => Path.Combine(PathArr.Append(File2023).ToArray());
}
