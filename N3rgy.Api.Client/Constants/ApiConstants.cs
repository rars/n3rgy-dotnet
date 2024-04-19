namespace N3rgy.Api.Client.Constants;

internal static class ApiConstants
{
    public static readonly string Electricity = "electricity";
    public static readonly string Gas = "gas";
    public static readonly string Consumption = "consumption";
    public static readonly string Tariff = "tariff";

    /// <summary>
    /// A special value indicating that consumption data for gas is missing.
    /// </summary>
    public static readonly decimal MissingGasEnergyConsumptionM3 = 16777.215m;
}
