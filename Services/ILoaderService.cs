namespace Services;

public interface ILoaderService
{
  public bool TransferData<T>(string sheetId, string range)
  where T : class, new();
}
