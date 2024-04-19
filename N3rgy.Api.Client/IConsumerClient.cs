namespace N3rgy.Api.Client;

using N3rgy.Api.Client.Data;

public interface IConsumerClient
{
    public Task<IReadOnlyList<ElectricityConsumptionRecord>> GetElectricityConsumption(
        DateRange dateRange,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<GasConsumptionRecord>> GetGasConsumption(
        DateRange dateRange,
        bool filterMissing = true,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<ElectricityTariffRecord>> GetElectricityTariff(
        DateRange dateRange,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<GasTariffRecord>> GetGasTariff(
        DateRange dateRange,
        CancellationToken cancellationToken = default);
}
