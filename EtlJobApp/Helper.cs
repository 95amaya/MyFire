using Newtonsoft.Json;

namespace EtlJobApp;
public static class Helper
{

    public static T? ReadFromJson<T>(string path)
    where T : class, new()
    {
        using StreamReader file = new(path);
        string json = file.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static void WriteToJson(string path, object obj)
    {
        var jsonObj = JsonConvert.SerializeObject(obj);

        using StreamWriter file = new(path);
        file.Write(jsonObj);
    }

}