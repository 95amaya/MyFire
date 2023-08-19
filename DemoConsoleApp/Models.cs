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
}

