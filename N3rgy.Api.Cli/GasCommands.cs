namespace N3rgy.Api.Cli;

using Cocona;
using N3rgy.Api.Client;

public sealed class GasCommands : AbstractCommands
{
    private readonly IConsumerClient _consumerClient;

    public GasCommands(
        IConsumerClient consumerClient)
    {
        _consumerClient = consumerClient;
    }

    [Command(Description = "Retrieves gas consumption data.")]
    public Task Consumption(
        DateOnly? startDate,
        DateOnly? endDate,
        string? outputFile)
        => ProcessRecords(() => _consumerClient.GetGasConsumption(GetDateRange(startDate, endDate)), outputFile);

    [Command(Description = "Retrieves gas tariff data.")]
    public Task Tariff(
        DateOnly? startDate,
        DateOnly? endDate,
        string? outputFile)
        => ProcessRecords(() => _consumerClient.GetGasTariff(GetDateRange(startDate, endDate)), outputFile);
}
