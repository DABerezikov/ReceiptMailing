using Microsoft.Extensions.DependencyInjection;

namespace ReceiptMailing.ViewModels
{
    internal class ViewModelLocator
    {
        public MainWindowViewModel MainWindowModel => App.Services.GetRequiredService<MainWindowViewModel>();
        public PdfSplitterViewModel PdfSplitterWindowModel => App.Services.GetRequiredService<PdfSplitterViewModel>();
        public ImportDBViewModel ImportDBViewModel => App.Services.GetRequiredService<ImportDBViewModel>();

        public EditGardenerViewModel EditGardenerViewModel => App.Services.GetRequiredService<EditGardenerViewModel>();

    }
}
