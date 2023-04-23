using Microsoft.Extensions.DependencyInjection;
using ReceiptMailing.Data.Context;
using ReceiptMailing.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ReceiptMailing.Services
{
    internal static class ServiceRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
           .AddTransient<IDataService, DataService>()
           .AddTransient<IUserDialog, UserDialog>()
           .AddTransient<ReceiptsSplitter>()
           .AddTransient<IMailService, MailService>()
           
        ;
    }
}
