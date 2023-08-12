using System;
using System.IO;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Newtonsoft.Json;

namespace Demo;

public static class Helper
{
    public static SheetsService InitializeSheetService(string appName, string[] scopes)
    {
        UserCredential credential;

        using (var stream =
            new FileStream("./DemoConsoleApp/credentials.json", FileMode.Open, FileAccess.Read))
        {
            // The file token.json stores the user's access and refresh tokens, and is created
            // automatically when the authorization flow completes for the first time.
            string credPath = "./DemoConsoleApp/token.json";
            credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.FromStream(stream).Secrets,
                scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true)).Result;
            Console.WriteLine("Credential file saved to: " + credPath);
        }

        // Create Google Sheets API service.
        return new SheetsService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential,
            ApplicationName = appName,
        });
    }

    public static T ReadFromJson<T>(string path)
    where T : class, new()
    {
        using StreamReader file = new(path);
        try
        {
            string json = file.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception)
        {
            Console.WriteLine("Problem reading file");
            return new T();
        }
    }

    public static void WriteToJson(string path, object obj)
    {
        var jsonObj = JsonConvert.SerializeObject(obj);

        using StreamWriter file = new(path);
        try
        {
            file.Write(jsonObj);
        }
        catch (Exception)
        {
            Console.WriteLine("Problem reading file");
        }

    }

}