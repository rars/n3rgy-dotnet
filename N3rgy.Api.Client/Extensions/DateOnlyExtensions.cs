namespace N3rgy.Api.Client.Extensions;

internal static class DateOnlyExtensions
{
    public static string ToDateString(this DateOnly self)
        => self.ToString("yyyyMMdd");
}
