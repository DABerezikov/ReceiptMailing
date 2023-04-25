using ReceiptMailing.Infrastructure.Commands.Base;
using ReceiptMailing.Views.Windows;

namespace ReceiptMailing.Infrastructure.Commands
{
    internal class PdfSplitterCommand : Command
    {
        protected override void Execute(object p)
        {
            var pdfSplitter_window = new PdfSplitterWindow();
            pdfSplitter_window.Show();
        }
    }
}
