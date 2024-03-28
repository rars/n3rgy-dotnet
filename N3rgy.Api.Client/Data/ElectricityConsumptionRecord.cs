namespace N3rgy.Api.Client.Data;

using CsvHelper.Configuration.Attributes;

public sealed record ElectricityConsumptionRecord
{
    [Name("timestamp (UTC)")]
    public DateTime Timestamp { get; init; }

    [Name("energyConsumption (kWh)")]
    public decimal EnergyConsumptionKwh { get; init; }
}
