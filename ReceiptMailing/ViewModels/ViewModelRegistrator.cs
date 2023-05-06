using Microsoft.Extensions.DependencyInjection;

namespace ReceiptMailing.ViewModels
{
    internal static class ViewModelRegistrator
    {
        public static IServiceCollection AddViews(this IServiceCollection services) => services
           .AddTransient<MainWindowViewModel>()
           .AddTransient<PdfSplitterViewModel>()
           .AddTransient<ImportDBViewModel>()
           .AddTransient<EditGardenerViewModel>()
        ;
    }
}