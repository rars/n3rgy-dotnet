.NET CLI client for retrieving n3rgy consumer data.

## Installation

Set the `N3RGY__APIKEY` environment variable to your In Home Display (IHD) MAC address (without hyphens). This acts as the security key for the n3rgy API, e.g.

```bash
set N3RGY__APIKEY="0123456789ABCDEF"
```

## CLI

You can retrieve consumption and tariff data using the commands below. Note that you can substitute `gas` for `electricity`:
```bash
n3rgy electricity consumption --start-date 2024-04-20 --end-date 2024-04-24 --output-file electricity_consumption.csv
```
or
```bash
n3rgy electricity tariff --start-date 2024-04-20 --end-date 2024-04-24 --output-file electricity_tariff.csv
```

Display inline help:
```bash
n3rgy --help
```
