namespace Services.CoreLibraries;

public interface ICsvWriter
{
    public int Write(string path, IEnumerable<ICsvRecord> list);
}

public interface ICsvRecord
{
    public string GetCsvHeader();
    public string GetCsvRow();
}
