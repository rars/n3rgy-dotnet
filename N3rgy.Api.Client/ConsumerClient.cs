namespace N3rgy.Api.Client;

using System.Globalization;
using CsvHelper;
using Flurl;
using Flurl.Http;
using N3rgy.Api.Client.Authorization;
using N3rgy.Api.Client.Constants;
using N3rgy.Api.Client.Data;
using N3rgy.Api.Client.Extensions;

public sealed class ConsumerClient : IConsumerClient
{
    private readonly string _baseUrl;
    private readonly IN3rgyAuthorizationProvider _authorizationProvider;

    public ConsumerClient(
        IN3rgyAuthorizationProvider authorizationProvider,
        string baseUrl = "https://consumer-api.data.n3rgy.com")
    {
        _baseUrl = baseUrl;
        _authorizationProvider = authorizationProvider;
    }

    /// <summary>
    /// Retrieve electricity consumption data for the specified date range.
    /// </summary>
    /// <param name="dateRange">The date range to retrieve data for.</param>
    /// <param name="cancellationToken">For signalling cancellation.</param>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public Task<IReadOnlyList<ElectricityConsumptionRecord>> GetElectricityConsumption(
        DateRange dateRange,
        CancellationToken cancellationToken = default)
        => GetRecordsImpl<ElectricityConsumptionRecord>(ApiConstants.Electricity, ApiConstants.Consumption, dateRange, cancellationToken);

    public async Task<IReadOnlyList<GasConsumptionRecord>> GetGasConsumption(
        DateRange dateRange,
        bool filterMissing = true,
        CancellationToken cancellationToken = default)
    {
        var records = await GetRecordsImpl<GasConsumptionRecord>(ApiConstants.Gas, ApiConstants.Consumption, dateRange, cancellationToken);

        return filterMissing ? records.Where(x => x.EnergyConsumptionM3 != ApiConstants.MissingGasEnergyConsumptionM3).ToList() : records;
    }

    public Task<IReadOnlyList<ElectricityTariffRecord>> GetElectricityTariff(
        DateRange dateRange,
        CancellationToken cancellationToken = default)
        => GetRecordsImpl<ElectricityTariffRecord>(ApiConstants.Electricity, ApiConstants.Tariff, dateRange, cancellationToken);

    public Task<IReadOnlyList<GasTariffRecord>> GetGasTariff(
        DateRange dateRange,
        CancellationToken cancellationToken = default)
        => GetRecordsImpl<GasTariffRecord>(ApiConstants.Gas, ApiConstants.Tariff, dateRange, cancellationToken);

    public async Task<IReadOnlyList<TRecord>> GetRecordsImpl<TRecord>(
        string energyType,
        string readingType,
        DateRange dateRange,
        CancellationToken cancellationToken)
    {
        var result = await $"{_baseUrl}/{energyType}/{readingType}/1"
            .SetQueryParams(new { start = dateRange.StartDate.ToDateString(), end = dateRange.EndDate.ToDateString(), output = "csv" })
            .WithHeader("Authorization", await _authorizationProvider.GetAuthorization(cancellationToken))
            .GetStreamAsync(cancellationToken: cancellationToken);

        using var streamReader = new StreamReader(result);
        using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        return csv.GetRecords<TRecord>().ToList();
    }
}
