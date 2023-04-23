using Microsoft.Extensions.DependencyInjection;

namespace ReceiptMailing.ViewModels
{
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViews(this IServiceCollection services) => services
           .AddSingleton<MainWindowViewModel>()
           .AddTransient<PdfSplitterViewModel>()
           .AddTransient<ImportDBViewModel>()
        ;
    }
}