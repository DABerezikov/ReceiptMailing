using Microsoft.Extensions.DependencyInjection;
using ReceiptMailing.Infrastructure.Commands.Base;
using ReceiptMailing.ViewModels;
using ReceiptMailing.Views.Windows;

namespace ReceiptMailing.Infrastructure.Commands;

internal class GardenersWindowCommand : Command
{
    protected override void Execute(object? p)
    {
        var viewModel = App.Services.GetRequiredService<GardenersViewModel>();
        var window = new GardenersWindow { DataContext = viewModel };
        window.ShowDialog();
    }
}
