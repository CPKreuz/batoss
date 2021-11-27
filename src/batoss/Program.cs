using batoss.Helpers;
using batoss.Options;
using batoss.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

DebugConsole();

HostBuilder builder = new();

builder
    .ConfigureAppConfiguration(config =>
    {
        config.AddJsonFile(PathHelpers.GetAppDataPath("global.json"), true);
        config.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "project.json"));
        config.AddJsonFile(Path.Combine(Environment.CurrentDirectory, "project.json.u"), true);
        config.AddCommandLine(args);
    })
    .ConfigureLogging(logging =>
    {
        logging
            .AddSimpleConsole(config =>
            {
                config.SingleLine = true;
            });
    })
    .ConfigureServices((context, services) =>
    {
        services
            .AddOptions()
            .Configure<ProfilesOptions>(context.Configuration.GetSection("Profiles"))
            .Configure<WslOptions>(context.Configuration.GetSection("Wsl"));

        services
            .AddSingleton<CompilerCommandFactory>()
            .AddSingleton<CommandFactory>()
            .AddSingleton<ProfileProvider>()
            .AddSingleton<CompilerFilesProvider>()
            .AddSingleton<CompilerService>()
            .AddSingleton<LinkingService>();
    });

IHost host = builder.Start();

CompilerService compiler = host.Services.GetRequiredService<CompilerService>();
if (!compiler.Compile())
{
    return;
}

LinkingService linker = host.Services.GetRequiredService<LinkingService>();
if (!linker.Link())
{
    return;
}

[System.Diagnostics.Conditional("DEBUG")]
static void DebugConsole()
{
    Console.WriteLine("Press Enter to launch without debugger");

    if (Console.ReadKey().Key != ConsoleKey.Enter)
    {
        System.Diagnostics.Debugger.Launch();
    }
}