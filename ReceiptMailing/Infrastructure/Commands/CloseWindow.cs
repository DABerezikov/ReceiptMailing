using ReceiptMailing.Infrastructure.Commands.Base;
using System.Windows;
using System.Linq;

namespace ReceiptMailing.Infrastructure.Commands
{
    internal class CloseWindow : Command
    {
        private static Window? GetWindow(object p) => p as Window 
                                                      ?? Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused)
                                                      ?? Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);

        protected override bool CanExecute(object p) => GetWindow(p) != null;

        protected override void Execute(object p) => GetWindow(p)?.Close();
    }
}
