﻿// See https://aka.ms/new-console-template for more information
using EtlJobApp;
using Serilog;

using var log = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var secrets = Helper.ReadFromJson<EtlJobApp.Models.Secrets>("./secrets.json"); //"./EtlJobApp/secrets.json"
if (secrets == null)
{
    throw new ArgumentNullException(nameof(secrets), "Needs to have values");
}

log.Information("Hello, Serilog!");
log.Information("secrets: {@Secrets}", secrets);
BillTransactionEtlJob.Run(log, secrets);


