using Microsoft.Extensions.DependencyInjection;
using ReceiptMailing.Infrastructure.Commands.Base;
using ReceiptMailing.ViewModels;
using ReceiptMailing.Views.Windows;

namespace ReceiptMailing.Infrastructure.Commands;

internal class MailSettingsCommand : Command
{
    protected override void Execute(object? p)
    {
        var viewModel = App.Services.GetRequiredService<MailSettingsViewModel>();
        var window    = new MailSettingsWindow { DataContext = viewModel };
        window.ShowDialog();
    }
}
