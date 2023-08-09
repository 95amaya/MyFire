using CoreLibraries;

namespace Services;

public class LoaderService : ILoaderService
{
  private ISheetReader _sheetReader { get; set; }

  public LoaderService(ISheetReader sheetReader)
  {
    _sheetReader = sheetReader;
  }

  public bool TransferData<T>(string sheetId, string range)
  where T : class, new()
  {
    var sheetData = _sheetReader.ReadFrom<T>(sheetId, range);


    return false;
  }

}
