using System;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels;
using ReceiptMailing.Views.Windows;

namespace ReceiptMailing.Services;

public class UserDialog : IUserDialog
{
    /// <summary>Активное окно приложения</summary>
    protected static Window? ActiveWindow => Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);

    /// <summary>Окно с фокусом ввода</summary>
    protected static Window? FocusedWindow => Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);

    /// <summary>Текущее окно приложения</summary>
    protected static Window? CurrentWindow => FocusedWindow ?? ActiveWindow;

    /// <summary>Открыть диалога выбора файла для чтения</summary>
    public virtual FileInfo? OpenFile(string title, string filter = "Исходные файлы (*.pdf, *.xls, *.xlsx)|*.pdf; *.xls; *.xlsx|" +
                                                                    " PDF(*.pdf)|*.pdf| Excel(*.xls,*.xlsx)|*.xls;*.xlsx|" +
                                                                    " Все файлы (*.*)|*.*", string? defaultFilePath = null)
    {
        var dialog = new OpenFileDialog
        {
            Title = title,
            RestoreDirectory = true,
            Filter = filter ?? throw new ArgumentNullException(nameof(filter)),
        };
        if (defaultFilePath is { Length: > 0 })
            dialog.FileName = defaultFilePath;

        return dialog.ShowDialog(CurrentWindow) == true
            ? new(dialog.FileName)
            : defaultFilePath is null ? null : new(defaultFilePath);
    }

    /// <summary>Открыть диалога выбора файла для записи</summary>
    public virtual FileInfo? SaveFile(string title, string filter = "Все файлы (*.*)|*.*", string? defaultFilePath = null)
    {
        var dialog = new SaveFileDialog
        {
            Title = title,
            RestoreDirectory = true,
            Filter = filter ?? throw new ArgumentNullException(nameof(filter)),
        };
        if (defaultFilePath is { Length: > 0 })
            dialog.FileName = defaultFilePath;

        return dialog.ShowDialog(CurrentWindow) == true
            ? new(dialog.FileName)
            : defaultFilePath is null ? null : new(defaultFilePath);
    }

    /// <summary>Диалог с текстовым вопросом и вариантами выбора Yes/No</summary>
    public virtual bool YesNoQuestion(string text, string title = "Вопрос...")
    {
        var result = CurrentWindow is null
            ? MessageBox.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question)
            : MessageBox.Show(CurrentWindow, text, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
        return result == MessageBoxResult.Yes;
    }

    /// <summary>Диалог с текстовым вопросом и вариантами выбора Ok/Cancel</summary>
    public virtual bool OkCancelQuestion(string text, string title = "Вопрос...")
    {
        var result = CurrentWindow is null
            ? MessageBox.Show(text, title, MessageBoxButton.OKCancel, MessageBoxImage.Question)
            : MessageBox.Show(CurrentWindow, text, title, MessageBoxButton.OKCancel, MessageBoxImage.Question);
        return result == MessageBoxResult.OK;
    }

    /// <summary>Диалог с информацией</summary>
    public virtual void Information(string text, string title = "Вопрос...")
    {
        if (CurrentWindow is null)
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Information);
        else
            MessageBox.Show(CurrentWindow, text, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }

    /// <summary>Диалог с предупреждением</summary>
    public virtual void Warning(string text, string title = "Вопрос...")
    {
        if (CurrentWindow is null)
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        else
            MessageBox.Show(CurrentWindow, text, title, MessageBoxButton.OK, MessageBoxImage.Warning);
    }

    /// <summary>Диалог с ошибкой</summary>
    public virtual void Error(string text, string title = "Вопрос...")
    {
        if (CurrentWindow is null)
            MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Error);
        else
            MessageBox.Show(CurrentWindow, text, title, MessageBoxButton.OK, MessageBoxImage.Error);
    }

    public virtual bool CreateOrEditGardener(Gardener gardener)
    {
        var tempGardener = gardener;
        var viewModel = new EditGardenerViewModel(tempGardener);
        var window = new EditGardenerWindow { DataContext = viewModel };
        var result = window.ShowDialog();
        return result ?? false;
    }

    public virtual bool CreateOrEditParcel(Parcel parcel, IRepository<Gardener> gardenerRepository)
    {
        var tempParcel = parcel;
        var viewModel = new EditParcelViewModel(tempParcel, gardenerRepository);
        var window = new EditParcelWindow { DataContext = viewModel };
        var result = window.ShowDialog();
        return result ?? false;
    }
}
