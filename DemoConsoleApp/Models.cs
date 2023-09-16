using System.Collections.Generic;
using System.IO;
using System.Linq;
using Services.Models;

namespace DemoConsoleApp.Models;

public class Secrets
{
    public List<BillTransactionSheet> BillTransactionSheets { get; set; }
    public string ConnectionString { get; set; }
    public IEnumerable<string> TempFilePathArr { get; set; }

    private List<string> _currentDirArr = new() { Directory.GetCurrentDirectory() };
    public string TempFilePath => Path.Combine(_currentDirArr.Concat(TempFilePathArr).ToArray());

    public BillTransactionImport ImportFiles { get; set; }
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