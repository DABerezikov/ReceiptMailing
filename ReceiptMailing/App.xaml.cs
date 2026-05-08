using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReceiptMailing.Data;
using ReceiptMailing.Data.Context;
using ReceiptMailing.Data.Repositories;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels;

namespace ReceiptMailing;

public partial class App
{
    [field: AllowNull, MaybeNull]
    public static IHost Host => field ??= Program
        .CreateHostBuilder(Environment.GetCommandLineArgs())
        .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("appsettings.json", true, true))
        .Build();

    public static IServiceProvider Services => Host.Services;

    protected override async void OnStartup(StartupEventArgs e)
    {
        var host = Host;

        using var scope = Services.CreateScope();
        scope.ServiceProvider.GetRequiredService<ParcelDbInitializer>().Initialize();

        base.OnStartup(e);
        await host.StartAsync();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        base.OnExit(e);
        using var host = Host;
        await host.StopAsync();
    }

    internal static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
        .Configure<MailSettings>(host.Configuration.GetSection(nameof(MailSettings)))
        .Configure<MessageSettings>(host.Configuration.GetSection(nameof(MessageSettings)))
        .AddViews()
        .AddServices()
        .AddDbContext<ParcelDb>(
            opt => opt
                .UseSqlite(
                    host.Configuration.GetConnectionString("Data")))
        .AddTransient(typeof(IRepository<>), typeof(DbRepository<>))
        .AddTransient(typeof(IParcelRepository<>), typeof(DbParcelsRepository<>))
        .AddTransient(typeof(IGardenerRepository<>), typeof(DbGardenersRepository<>))
        .AddTransient<ParcelDbInitializer>();
}
