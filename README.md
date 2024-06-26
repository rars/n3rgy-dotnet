# n3rgy .NET client and CLI

.NET client and CLI for n3rgy consumer data.

## Installation

Set the `N3RGY__APIKEY` environment variable to your In Home Display (IHD) MAC address (without hyphens). This acts as the security key for the n3rgy API, e.g.

```
set N3RGY__APIKEY="0123456789ABCDEF"
```

## CLI

You can retrieve consumption and tariff data using the commands below. Note that you can substitute `gas` for `electricity`:
```
n3rgy electricity consumption --start-date 2024-04-20 --end-date 2024-04-24 --output-file electricity_consumption.csv
```

```
n3rgy electricity tariff --start-date 2024-04-20 --end-date 2024-04-24 --output-file electricity_tariff.csv
```