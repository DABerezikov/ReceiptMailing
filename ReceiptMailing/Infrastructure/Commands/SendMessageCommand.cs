using Microsoft.Extensions.DependencyInjection;
using ReceiptMailing.Infrastructure.Commands.Base;
using ReceiptMailing.ViewModels;
using ReceiptMailing.Views.Windows;

namespace ReceiptMailing.Infrastructure.Commands;

internal class SendMessageCommand : Command
{
    protected override void Execute(object? p)
    {
        var viewModel = App.Services.GetRequiredService<SendMessageViewModel>();
        var window    = new SendMessageWindow { DataContext = viewModel };
        window.ShowDialog();
    }
}
