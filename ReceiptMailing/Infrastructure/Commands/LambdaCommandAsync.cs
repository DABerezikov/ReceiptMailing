using ReceiptMailing.Infrastructure.Commands.Base;
using System;

namespace ReceiptMailing.Infrastructure.Commands
{
    internal class LambdaCommandAsync : Command
    {
        private readonly ActionAsync<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public LambdaCommandAsync(ActionAsync execute, Func<bool> canExecute = null)
            : this(async p => await execute(), canExecute is null ? (Func<object, bool>)null : p => canExecute())
        {

        }

        public LambdaCommandAsync(ActionAsync<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        protected override bool CanExecute(object p) => _canExecute?.Invoke(p) ?? true;

        protected override void Execute(object p) => _execute(p);
    }
}