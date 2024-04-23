namespace N3rgy.Api.Cli;

using System.Globalization;
using CsvHelper;
using N3rgy.Api.Client.Data;

public abstract class AbstractCommands
{
    protected DateRange GetDateRange(DateOnly? startDate, DateOnly? endDate)
    {
        endDate ??= DateOnly.FromDateTime(DateTime.Now);
        startDate ??= endDate?.AddDays(-90);
        return new DateRange(startDate!.Value, endDate!.Value);
    }

    protected async Task ProcessRecords<TRecord>(
        Func<Task<IReadOnlyList<TRecord>>> getRecords,
        string? outputFile)
    {
        var records = await getRecords();

        if (outputFile is null)
        {
            WriteToStdOut(records);
        }
        else
        {
            Console.Write($"Writing {records.Count} records to {outputFile}...");
            WriteToCsv(records, outputFile);
            Console.WriteLine(" Done.");
        }
    }

    private void WriteToCsv<TRecord>(
        IReadOnlyList<TRecord> records,
        string outputFile)
    {
        using var writer = new StreamWriter(outputFile);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(records);
    }

    private void WriteToStdOut<TRecord>(
        IReadOnlyList<TRecord> records)
    {
        using var writer = new StringWriter();
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(records);
        csv.Flush();

        Console.WriteLine(writer.ToString());
    }
}
