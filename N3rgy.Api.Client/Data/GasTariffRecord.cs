namespace N3rgy.Api.Client.Data;

using CsvHelper.Configuration.Attributes;

public sealed record GasTariffRecord
{
    [Name("timestamp (UTC)")]
    public DateTime Timestamp { get; init; }

    [Name("chargeType")]
    public string? ChargeType { get; init; }

    [Name("standingCharge (pence per day)")]
    public decimal? StandingChargePence { get; init; }

    [Name("touPrice (pence per kWh)")]
    public decimal? TouPricePence { get; init; }

    [Name("blockPrice1 (pence per kWh)")]
    public decimal? Block1PricePence { get; init; }

    [Name("blockPrice2 (pence per kWh)")]
    public decimal? Block2PricePence { get; init; }

    [Name("blockPrice3 (pence per kWh)")]
    public decimal? Block3PricePence { get; init; }

    [Name("blockPrice4 (pence per kWh)")]
    public decimal? Block4PricePence { get; init; }

    [Name("blockThreshold1 (kWh)")]
    public decimal? BlockThreshold1 { get; init; }

    [Name("blockThreshold2 (kWh)")]
    public decimal? BlockThreshold2 { get; init; }

    [Name("blockThreshold3 (kWh)")]
    public decimal? BlockThreshold3 { get; init; }
}
