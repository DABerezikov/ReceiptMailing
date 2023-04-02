using Microsoft.Extensions.DependencyInjection;
using ReceiptMailing.Services.Interfaces;

namespace ReceiptMailing.Services
{
    internal static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
           .AddTransient<IDataService, DataService>()
           .AddTransient<IUserDialog, UserDialog>()
        ;
    }
}
