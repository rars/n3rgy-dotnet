using System.Globalization;
using CsvHelper;
using Microsoft.Extensions.Configuration;
using N3rgy.Api.Client;
using N3rgy.Api.Client.Authorization;

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

static string GetInput(
    IConfiguration configuration,
    string key)
{
    var inputValue = configuration.GetValue<string>(key);

    if (inputValue is null)
    {
        Console.Write($"Missing argument {key}. Please enter value for {key} now and press enter: ");
        inputValue = Console.ReadLine()?.Trim();
    }

    return inputValue ?? throw new ArgumentException($"No value for {key} supplied.");
}

var authorizationProvider = new StaticN3rgyAuthorizationProvider(apiKey ?? throw new ArgumentException("Missing IHD MAC address. Please set env var 'N3RGY__APIKEY' to your IHD MAC address."));

var startDate = configuration.GetValue<DateOnly?>("StartDate") ?? throw new ArgumentException("Missing StartDate parameter. Please run with e.g. --StartDate 20240324.");
var endDate = configuration.GetValue<DateOnly?>("EndDate") ?? throw new ArgumentException("Missing EndDate parameter. Please run with e.g. --EndDate 20240328.");

var energyType = GetInput(configuration, "EnergyType");
var recordType = GetInput(configuration, "RecordType");
var outputFile = configuration.GetValue<string>("CsvOutFile");

var client = new ConsumerClient(authorizationProvider);

switch (energyType)
{
    case "electricity":
        {
            switch (recordType)
            {
                case "consumption":
                    {
                        await ProcessRecords(() => client.GetElectricityConsumption(startDate, endDate), outputFile);
                        break;
                    }

                case "tariff":
                    {
                        await ProcessRecords(() => client.GetElectricityTariff(startDate, endDate), outputFile);
                        break;
                    }

                default:
                    throw new ArgumentException($"Unknown RecordType: {recordType}. Must be one of 'consumption', 'tariff'.");
            }

            break;
        }

    case "gas":
        {
            switch (recordType)
            {
                case "consumption":
                    {
                        await ProcessRecords(() => client.GetGasConsumption(startDate, endDate), outputFile);
                        break;
                    }

                case "tariff":
                    {
                        await ProcessRecords(() => client.GetGasTariff(startDate, endDate), outputFile);
                        break;
                    }

                default:
                    throw new ArgumentException($"Unknown RecordType: {recordType}. Must be one of 'consumption', 'tariff'.");
            }

            break;
        }

    default:
        throw new ArgumentException($"Unknown EnergyType: {energyType}. Must be one of 'electricity', 'gas'.");
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
