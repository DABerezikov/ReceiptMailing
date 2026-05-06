using System.Windows;
using System.Windows.Controls;
using ReceiptMailing.ViewModels;

namespace ReceiptMailing.Views.Windows;

public partial class MailSettingsWindow : Window
{
    public MailSettingsWindow()
    {
        InitializeComponent();
    }

    protected override void OnContentRendered(System.EventArgs e)
    {
        base.OnContentRendered(e);

        if (DataContext is not MailSettingsViewModel vm) return;

        // Заполняем PasswordBox из ViewModel (PasswordBox не поддерживает привязку данных)
        PasswordInput.Password = vm.Password;

        // Когда переключаемся обратно в режим "скрыто" — синхронизируем PasswordBox
        // из vm.Password (который мог измениться через открытый TextBox)
        vm.PropertyChanged += (_, args) =>
        {
            if (args.PropertyName == nameof(MailSettingsViewModel.IsPasswordVisible)
                && !vm.IsPasswordVisible)
                PasswordInput.Password = vm.Password;
        };

        // Подписываемся на запрос закрытия окна
        vm.CloseRequested += result => DialogResult = result;
    }

    private void PasswordInput_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is MailSettingsViewModel vm)
            vm.Password = ((PasswordBox)sender).Password;
    }
}
