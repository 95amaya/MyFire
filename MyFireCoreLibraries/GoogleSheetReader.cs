using AutoMapper;

namespace MyFireCoreLibraries;

public class GoogleSheetReader : ISheetReader
{
    private ISheetClient _googleSheetsApiClient { get; set; }
    private IMapper _mapper { get; set; }
    public GoogleSheetReader(IMapper mapper, ISheetClient googleSheetsApiClient)
    {
        _googleSheetsApiClient = googleSheetsApiClient;
        _mapper = mapper;
    }

    public IList<T> ReadFrom<T>(string spreadsheetId, string range)
    where T : class, new()
    {
        var rawValues = _googleSheetsApiClient.GetValues(spreadsheetId, range);

        return rawValues.SafeHasRows()
            ? _mapper.Map<IList<T>>(rawValues)
            : new List<T>();
    }
}
