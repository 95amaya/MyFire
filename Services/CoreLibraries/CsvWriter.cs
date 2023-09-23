using CoreLibraries;

namespace Services.CoreLibraries;

public class CsvWriter : ICsvWriter
{
    public CsvWriter()
    {
    }

    public int Write(string path, IEnumerable<ICsvRecord> list)
    {
        if (!list.SafeHasRows())
        {
            return 0;
        }

        if (!File.Exists(path))
        {
            using var sw = new StreamWriter(path);
            sw.WriteLine(list.FirstOrDefault()?.GetCsvHeader());
        }

        using var writer = new StreamWriter(path, true);

        foreach (var item in list)
        {
            writer.WriteLine(item.GetCsvRow());
        }

        return list.Count();
    }
}
