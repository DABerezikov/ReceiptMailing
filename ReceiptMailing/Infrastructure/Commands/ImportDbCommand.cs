using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptMailing.Infrastructure.Commands.Base;
using ReceiptMailing.Views.Windows;

namespace ReceiptMailing.Infrastructure.Commands
{
    internal class ImportDbCommand : Command
    {
        protected override void Execute(object p)
        {
            var import_window = new ImportDBWindow();
            import_window.Show();
        }
    }
}
