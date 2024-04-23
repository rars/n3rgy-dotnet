.NET n3rgy client for retrieving data.

```cs
using N3rgy.Api.Client;
using N3rgy.Api.Client.Authorization;

// ...

string GetApiKey(IServiceProvider serviceProvider)
{
    // Retrieve your API key from configuration:
    IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
    string? apiKey = configuration.GetValue<string>("N3RGY:APIKEY");

    // validate api key is set

    return apiKey;
}

services.AddSingleton<IN3rgyAuthorizationProvider>(sp => new StaticN3rgyAuthorizationProvider(GetApiKey(sp)));
services.AddSingleton<IConsumerClient>(sp => new ConsumerClient(sp.GetRequiredService<IN3rgyAuthorizationProvider>()));

// You can then use the IConsumerClient to retrieve gas or electricity data:
await _consumerClient.GetGasConsumption(dateRange);
```