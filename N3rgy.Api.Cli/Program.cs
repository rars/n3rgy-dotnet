using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Cocona;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N3rgy.Api.Client;
using N3rgy.Api.Client.Authorization;
using N3rgy.Api.Client.Data;

IConfiguration configuration = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

string? apiKey = configuration.GetValue<string>("N3RGY:APIKEY");

if (apiKey is null)
{
    Console.WriteLine("Please set the environment variable 'N3RGY__APIKEY' to your In Home Display (IHD) MAC address to avoid manually inputting this value below.");
    Console.Write("Please enter your IHD MAC here and press enter: ");
    apiKey = Console.ReadLine()?.Trim();
}

var builder = CoconaApp.CreateBuilder();

builder.Services.AddSingleton<IN3rgyAuthorizationProvider>(new StaticN3rgyAuthorizationProvider(apiKey!));
builder.Services.AddSingleton<IConsumerClient>(sp => new ConsumerClient(sp.GetRequiredService<IN3rgyAuthorizationProvider>()));

var app = builder.Build();

app.AddSubCommand(
    "electricity",
    x =>
    {
        x.AddCommand(
            "consumption",
            async (DateOnly? startDate, DateOnly? endDate, string? outputFile, IConsumerClient client) =>
                await ProcessRecords(() => client.GetElectricityConsumption(GetDateRange(startDate, endDate)), outputFile))
         .WithDescription("Retrieves electricity consumption data.");

        x.AddCommand(
            "tariff",
            async (DateOnly? startDate, DateOnly? endDate, string? outputFile, IConsumerClient client) =>
                await ProcessRecords(() => client.GetElectricityTariff(GetDateRange(startDate, endDate)), outputFile))
         .WithDescription("Retrieves electricity tariff data.");
    })
    .WithDescription("Retrieves data related to electricity energy usage.");

app.AddSubCommand(
    "gas",
    x =>
    {
        x.AddCommand(
            "consumption",
            async (DateOnly? startDate, DateOnly? endDate, string? outputFile, IConsumerClient client) =>
                await ProcessRecords(() => client.GetGasConsumption(GetDateRange(startDate, endDate)), outputFile))
         .WithDescription("Retrieves gas consumption data.");

        x.AddCommand(
            "tariff",
            async (DateOnly? startDate, DateOnly? endDate, string? outputFile, IConsumerClient client) =>
                await ProcessRecords(() => client.GetGasTariff(GetDateRange(startDate, endDate)), outputFile))
         .WithDescription("Retrieves gas tariff data.");
    })
    .WithDescription("Retrieves data related to gas energy usage.");

app.AddCommand(
    "version",
    () =>
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var productVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;
        Console.WriteLine(productVersion);
    })
    .WithDescription("Outputs the product version of this tool.");

await app.RunAsync();

static DateRange GetDateRange(DateOnly? startDate, DateOnly? endDate)
{
    endDate ??= DateOnly.FromDateTime(DateTime.Now);
    startDate ??= endDate?.AddDays(-90);
    return new DateRange(startDate!.Value, endDate!.Value);
}

static async Task ProcessRecords<TRecord>(
    Func<Task<IReadOnlyList<TRecord>>> getRecords,
    string? outputFile)
{
    var records = await getRecords();

    if (outputFile is null)
    {
        WriteToStdOut(records);
    }
    else
    {
        Console.Write($"Writing {records.Count} records to {outputFile}...");
        WriteToCsv(records, outputFile);
        Console.WriteLine(" Done.");
    }
}

static void WriteToCsv<TRecord>(
    IReadOnlyList<TRecord> records,
    string outputFile)
{
    using var writer = new StreamWriter(outputFile);
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

    csv.WriteRecords(records);
}

static void WriteToStdOut<TRecord>(
    IReadOnlyList<TRecord> records)
{
    using var writer = new StringWriter();
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

    csv.WriteRecords(records);
    csv.Flush();

    Console.WriteLine(writer.ToString());
}
