namespace N3rgy.Api.Client;

using N3rgy.Api.Client.Data;

internal interface IConsumerClient
{
    public Task<IReadOnlyList<ElectricityConsumptionRecord>> GetElectricityConsumption(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<GasConsumptionRecord>> GetGasConsumption(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<ElectricityTariffRecord>> GetElectricityTariff(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<GasTariffRecord>> GetGasTariff(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default);
}
