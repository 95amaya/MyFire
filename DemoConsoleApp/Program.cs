using DemoConsoleApp.Demos;

namespace DemoConsoleApp;
class Program
{
    static void Main(string[] args)
    {
        var secrets = Helper.ReadFromJson<Models.Secrets>("./DemoConsoleApp/secrets.json");
        // GoogleSheetsDemo.Run(args);
        // GoogleSheetsServicesDemo.Run(args);
        // BillTransactionImportDemo.Run(args, secrets);
    }
}
