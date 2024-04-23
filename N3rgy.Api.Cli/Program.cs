namespace N3rgy.Api.Cli;

using System.Diagnostics;
using System.Reflection;
using Cocona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using N3rgy.Api.Client;
using N3rgy.Api.Client.Authorization;

[HasSubCommands(typeof(ElectricityCommands), "electricity", Description = "Retrieves data related to electricity energy usage.")]
[HasSubCommands(typeof(GasCommands), "gas", Description = "Retrieves data related to gas energy usage.")]
public sealed class Program
{
    public static string GetApiKey(IServiceProvider serviceProvider)
    {
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        string? apiKey = configuration.GetValue<string>("N3RGY:APIKEY");

        if (apiKey is not null)
        {
            return apiKey;
        }

        Console.WriteLine("Please set the environment variable 'N3RGY__APIKEY' to your In Home Display (IHD) MAC address to avoid manually inputting this value below.");
        Console.Write("Please enter your IHD MAC here and press enter: ");
        return Console.ReadLine()?.Trim() ?? "";
    }

    public static async Task Main(string[] args)
    {
        await CoconaApp.CreateHostBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<IN3rgyAuthorizationProvider>(sp => new StaticN3rgyAuthorizationProvider(GetApiKey(sp)));
                services.AddSingleton<IConsumerClient>(sp => new ConsumerClient(sp.GetRequiredService<IN3rgyAuthorizationProvider>()));
            })
            .RunAsync<Program>(args, options =>
            {
                options.EnableShellCompletionSupport = true;
            });
    }

    [Command(Description = "Outputs the product version of this tool.")]
    public static void Version()
    {
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        var productVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).ProductVersion;
        Console.WriteLine(productVersion);
    }
}
