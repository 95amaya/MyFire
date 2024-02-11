namespace EtlJobApp.Models;

public class Secrets
{
    public List<string> BillTransactionNoiseFilterList { get; set; } = new List<string>();
    public BillTransactionImport ImportFiles { get; set; } = new BillTransactionImport();
    public BillTransactionExport ExportFiles { get; set; } = new BillTransactionExport();
}

public class BillTransactionImport
{
    public IEnumerable<string> PathArr { get; set; } = new List<string>();
    public BillTransactionFiles Files { get; set; } = new BillTransactionFiles();

    public string NeedsCreditPath => Path.Combine(PathArr.Append(Files.NeedsCredit).ToArray());
    public string WantsCreditPath => Path.Combine(PathArr.Append(Files.WantsCredit).ToArray());
    public string NeedsDebitPath => Path.Combine(PathArr.Append(Files.NeedsDebit).ToArray());
    public string WantsDebitPath => Path.Combine(PathArr.Append(Files.WantsDebit).ToArray());
}

public class BillTransactionFiles
{
    public string NeedsCredit { get; set; } = string.Empty;
    public string WantsCredit { get; set; } = string.Empty;
    public string NeedsDebit { get; set; } = string.Empty;
    public string WantsDebit { get; set; } = string.Empty;
}

public class BillTransactionExport
{
    public IEnumerable<string> PathArr { get; set; } = new List<string>();
    public string File2023 { get; set; } = string.Empty;
    public string File2024 { get; set; } = string.Empty;

    public string DirPath => Path.Combine(PathArr.ToArray());

    public string FilePath => Path.Combine(PathArr.Append(File2024).ToArray());
}
