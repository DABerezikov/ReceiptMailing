using Microsoft.Extensions.DependencyInjection;

namespace ReceiptMailing.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
    }
}
