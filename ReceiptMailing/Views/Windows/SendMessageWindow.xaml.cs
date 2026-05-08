using System.Windows;
using ReceiptMailing.ViewModels;

namespace ReceiptMailing.Views.Windows;

public partial class SendMessageWindow : Window
{
    public SendMessageWindow()
    {
        InitializeComponent();
    }

    protected override void OnContentRendered(System.EventArgs e)
    {
        base.OnContentRendered(e);

        if (DataContext is SendMessageViewModel vm)
            vm.CloseRequested += result => DialogResult = result;
    }
}
