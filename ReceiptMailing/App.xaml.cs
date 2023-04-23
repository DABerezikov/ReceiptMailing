using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ReceiptMailing.Services;
using ReceiptMailing.ViewModels;
using System;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReceiptMailing.Data;
using ReceiptMailing.Data.Context;
using ReceiptMailing.Data.Repositories;
using ReceiptMailing.Services.Interfaces.Repositories;

namespace ReceiptMailing
{
    public partial class App
    {
       
        private static IHost __Host;

        public static IHost Host => __Host ??= Program
            .CreateHostBuilder(Environment.GetCommandLineArgs())
            .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("appsettings.json", true, true))
            .ConfigureServices((host, services) => services
                .Configure<MailSettings>(host.Configuration.GetSection(nameof(MailSettings)))
                .AddViews()
                .AddServices()
                .AddDbContext<ParcelDB>(
                    opt => opt
                        .UseSqlite(
                            host.Configuration.GetConnectionString("Data"),
                            o => o.MigrationsAssembly("Data")))
                .AddScoped(typeof(IRepository<>), typeof(DbRepository<>))
                .AddScoped(typeof(IParcelRepository<>), typeof(DbParcelsRepository<>))
                .AddScoped(typeof(IGardenerRepository<>), typeof(DbGardenersRepository<>))
                .AddTransient<ParcelDBInitializer>()
            )
            .Build();

        public static IServiceProvider Services => Host.Services;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;

            using (var scope = Services.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ParcelDBInitializer>().Initialize();
            }

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
            .AddViews()
            .AddServices()
            .AddDbContext<ParcelDB>(
                opt => opt
                    .UseSqlite(
                        host.Configuration.GetConnectionString("Data")))
            .AddScoped(typeof(IRepository<>), typeof(DbRepository<>))
            .AddScoped(typeof(IParcelRepository<>), typeof(DbParcelsRepository<>))
            .AddScoped(typeof(IGardenerRepository<>), typeof(DbGardenersRepository<>))
            .AddTransient<ParcelDBInitializer>()
        ;
    }
}
