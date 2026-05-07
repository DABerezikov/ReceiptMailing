using ReceiptMailing.Infrastructure.Commands.Base;
using ReceiptMailing.Views.Windows;

namespace ReceiptMailing.Infrastructure.Commands;

internal class ExportDbCommand : Command
{
    protected override void Execute(object? p)
    {
        var window = new ExportDBWindow();
        window.Show();
    }
}
