namespace N3rgy.Api.Client.Data;

using CsvHelper.Configuration.Attributes;

public sealed record GasConsumptionRecord
{
    [Name("timestamp (UTC)")]
    public DateTime Timestamp { get; init; }

    [Name("energyConsumption (m3)")]
    public decimal EnergyConsumptionM3 { get; init; }
}
