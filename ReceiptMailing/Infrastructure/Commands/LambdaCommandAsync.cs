using System;
using ReceiptMailing.Infrastructure.Commands.Base;

namespace ReceiptMailing.Infrastructure.Commands;

internal class LambdaCommandAsync(ActionAsync<object?> execute, Func<object?, bool>? canExecute = null)
    : Command
{
    public LambdaCommandAsync(ActionAsync execute, Func<bool>? canExecute = null)
        : this(async p => await execute(), canExecute is null ? null : p => canExecute())
    {
    }

    protected override bool CanExecute(object? p) => canExecute?.Invoke(p) ?? true;

    protected override void Execute(object? p) => execute(p);
}
