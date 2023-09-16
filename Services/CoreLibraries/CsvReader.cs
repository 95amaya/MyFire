using AutoMapper;

namespace Services.CoreLibraries;

public class CsvReader : ICsvReader
{
    private readonly IMapper _mapper;
    public CsvReader(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IList<T> Read<T>(string path, bool skipFirstRow = false) where T : class, new()
    {
        List<List<object>> recordList = new();
        using var reader = new StreamReader(path);
        while (!reader.EndOfStream)
        {
            List<object> record = new();
            var line = reader.ReadLine();

            if (skipFirstRow)
            {
                skipFirstRow = false;
                continue;
            }

            foreach (var value in line?.Split(',') ?? new string[] { })
            {
                // remove beginning and ending quotes "1" => 1
                if (value.Length > 1 && value.FirstOrDefault() == '"' && value.LastOrDefault() == '"')
                {
                    record.Add(value[1..^1]);
                }
                else
                {
                    record.Add(value);
                }
            }
            recordList.Add(record);
        }

        return recordList.Count() > 0 ? _mapper.Map<IList<T>>(recordList) : new List<T>();
    }

}
