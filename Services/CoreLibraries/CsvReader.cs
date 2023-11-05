using AutoMapper;

namespace Services.CoreLibraries;

public class CsvReader : ICsvReader
{
    private readonly IMapper _mapper;
    public CsvReader(IMapper mapper)
    {
        _mapper = mapper;
    }

    public IList<T> Read<T>(StreamReader reader, bool skipFirstRow = false) where T : class, new()
    {
        List<List<string>> recordList = new();
        while (!reader.EndOfStream)
        {
            List<string> record = new();
            var line = reader.ReadLine() ?? string.Empty;

            if (skipFirstRow)
            {
                skipFirstRow = false;
                continue;
            }

            if (line.Length <= 0)
            {
                continue;
            }

            var charIndex = 0;

            while (charIndex < line.Length)
            {
                if (line.ElementAt(charIndex) == '"')
                {
                    charIndex++; // skip " char
                    var endIndex = line.IndexOf('"', charIndex);
                    record.Add(line[charIndex..endIndex]);

                    charIndex = endIndex + 2; // skip ",
                }
                else
                {
                    var endIndex = line.IndexOf(',', charIndex);
                    if (endIndex < 0)
                    {
                        endIndex = line.Length;
                    }

                    record.Add(line[charIndex..endIndex]);

                    charIndex = endIndex + 1; // skip ,
                }
            }

            recordList.Add(record);
        }

        return recordList.Count() > 0 ? _mapper.Map<IList<T>>(recordList) : new List<T>();
    }
}
