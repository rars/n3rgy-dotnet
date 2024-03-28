﻿namespace N3rgy.Api.Client;

using System;
using System.Globalization;
using CsvHelper;
using Flurl;
using Flurl.Http;
using N3rgy.Api.Client.Authorization;
using N3rgy.Api.Client.Constants;
using N3rgy.Api.Client.Data;
using N3rgy.Api.Client.Extensions;

public sealed class ConsumerClient
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
    /// Retrieve
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
    public Task<IReadOnlyList<ElectricityConsumptionRecord>> GetElectricityConsumption(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
        => GetRecordsImpl<ElectricityConsumptionRecord>(ApiConstants.Electricity, ApiConstants.Consumption, startDate, endDate, cancellationToken);

    public Task<IReadOnlyList<GasConsumptionRecord>> GetGasConsumption(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
        => GetRecordsImpl<GasConsumptionRecord>(ApiConstants.Gas, ApiConstants.Consumption, startDate, endDate, cancellationToken);

    public Task<IReadOnlyList<ElectricityTariffRecord>> GetElectricityTariff(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
        => GetRecordsImpl<ElectricityTariffRecord>(ApiConstants.Electricity, ApiConstants.Tariff, startDate, endDate, cancellationToken);

    public Task<IReadOnlyList<GasTariffRecord>> GetGasTariff(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken = default)
        => GetRecordsImpl<GasTariffRecord>(ApiConstants.Gas, ApiConstants.Tariff, startDate, endDate, cancellationToken);

    public async Task<IReadOnlyList<TRecord>> GetRecordsImpl<TRecord>(
        string energyType,
        string readingType,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var result = await $"{_baseUrl}/{energyType}/{readingType}/1"
            .SetQueryParams(new { start = startDate.ToDateString(), end = endDate.ToDateString(), output = "csv" })
            .WithHeader("Authorization", await _authorizationProvider.GetAuthorization(cancellationToken))
            .GetStreamAsync(cancellationToken: cancellationToken);

        using var streamReader = new StreamReader(result);
        using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);

        return csv.GetRecords<TRecord>().ToList();
    }
}
