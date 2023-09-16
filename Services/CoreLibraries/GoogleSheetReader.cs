using AutoMapper;

namespace CoreLibraries;

public class GoogleSheetReader : ISheetReader
{
    private readonly ISheetClient _googleSheetsApiClient;
    private readonly IMapper _mapper;
    public GoogleSheetReader(IMapper mapper, ISheetClient googleSheetsApiClient)
    {
        _googleSheetsApiClient = googleSheetsApiClient;
        _mapper = mapper;
    }

    public IList<T> Read<T>(string spreadsheetId, string range)
    where T : class, new()
    {
        var rawValues = _googleSheetsApiClient.GetValues(spreadsheetId, range);

        return rawValues.SafeHasRows()
            ? _mapper.Map<IList<T>>(rawValues)
            : new List<T>();
    }
}
