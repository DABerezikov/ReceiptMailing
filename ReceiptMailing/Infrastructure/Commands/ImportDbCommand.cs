﻿using ReceiptMailing.Infrastructure.Commands.Base;
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
