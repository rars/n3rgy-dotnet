namespace N3rgy.Api.Cli;

using Cocona;
using N3rgy.Api.Client;

public sealed class ElectricityCommands : AbstractCommands
{
    private readonly IConsumerClient _consumerClient;

    public ElectricityCommands(
        IConsumerClient consumerClient)
    {
        _consumerClient = consumerClient;
    }

    [Command(Description = "Retrieves electricity consumption data.")]
    public Task Consumption(
        DateOnly? startDate,
        DateOnly? endDate,
        string? outputFile)
        => ProcessRecords(() => _consumerClient.GetElectricityConsumption(GetDateRange(startDate, endDate)), outputFile);

    [Command(Description = "Retrieves electricity tariff data.")]
    public Task Tariff(
        DateOnly? startDate,
        DateOnly? endDate,
        string? outputFile)
        => ProcessRecords(() => _consumerClient.GetElectricityTariff(GetDateRange(startDate, endDate)), outputFile);
}
