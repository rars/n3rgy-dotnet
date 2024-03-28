// See https://aka.ms/new-console-template for more information
using N3rgy.Api.Client;
using N3rgy.Api.Client.Authorization;

var authorizationProvider = new StaticN3rgyAuthorizationProvider(/* add authorization key here! */);

var client = new ConsumerClient(authorizationProvider);

var startDate = new DateOnly(2024, 3, 24);
var endDate = new DateOnly(2024, 3, 28);

{
    var tariff = await client.GetElectricityTariff(startDate, endDate);

    var consumption = await client.GetElectricityConsumption(startDate, endDate);

    Console.WriteLine($"{tariff.Count} tariff records");

    foreach (var cRecord in tariff)
    {
        Console.WriteLine($"{cRecord.Timestamp} - {cRecord.ChargeType} - {cRecord.StandingChargePence}p");
    }

    Console.WriteLine($"{consumption.Count} consumption records");

    foreach (var record in consumption)
    {
        Console.WriteLine($"{record.Timestamp} - {record.EnergyConsumptionKwh}");
    }
}

{
    var tariff = await client.GetGasTariff(startDate, endDate);

    var consumption = await client.GetGasConsumption(startDate, endDate);

    Console.WriteLine($"{tariff.Count} tariff records");

    foreach (var cRecord in tariff)
    {
        Console.WriteLine($"{cRecord.Timestamp} - {cRecord.ChargeType} - {cRecord.StandingChargePence}p");
    }

    Console.WriteLine($"{consumption.Count} consumption records");

    foreach (var record in consumption)
    {
        Console.WriteLine($"{record.Timestamp} - {record.EnergyConsumptionM3}");
    }
}
