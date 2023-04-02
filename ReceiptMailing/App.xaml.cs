using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ReceiptMailing.Services;
using ReceiptMailing.ViewModels;
using System;
using System.Linq;
using System.Windows;

namespace ReceiptMailing
{
    public partial class App
    {
       
        private static IHost __Host;

        public static IHost Host => __Host ??= Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder(Environment.GetCommandLineArgs())
            .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("appsettings.json", true, true))
            .ConfigureServices((host, services) => services
                .AddViews()
                .AddServices()
            )
            .Build();

        public static IServiceProvider Services => Host.Services;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            using var host = Host;
            await host.StopAsync();
        }
    }
}
