using System.IO;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Services.Interfaces.Repositories;

namespace ReceiptMailing.Services.Interfaces;

public interface IUserDialog
{
    /// <summary>Открыть диалога выбора файла для чтения</summary>
    FileInfo? OpenFile(string title, string filter = "Исходные файлы (*.pdf, *.xls, *.xlsx)|*.pdf; *.xls; *.xlsx|" +
                                                     " PDF(*.pdf)|*.pdf| Excel(*.xls,*.xlsx)|*.xls;*.xlsx|" +
                                                     " Все файлы (*.*)|*.*", string? defaultFilePath = null);

    /// <summary>Открыть диалога выбора файла для записи</summary>
    FileInfo? SaveFile(string title, string filter = "Все файлы (*.*)|*.*", string? defaultFilePath = null);

    /// <summary>Открыть диалог выбора папки</summary>
    string? OpenFolder(string title, string? defaultPath = null);

    /// <summary>Диалог с текстовым вопросом и вариантами выбора Yes/No</summary>
    bool YesNoQuestion(string text, string title = "Вопрос...");

    /// <summary>Диалог с текстовым вопросом и вариантами выбора Ok/Cancel</summary>
    bool OkCancelQuestion(string text, string title = "Вопрос...");

    /// <summary>Диалог с информацией</summary>
    void Information(string text, string title = "Вопрос...");

    /// <summary>Диалог с предупреждением</summary>
    void Warning(string text, string title = "Вопрос...");

    /// <summary>Диалог с ошибкой</summary>
    void Error(string text, string title = "Вопрос...");

    /// <summary>Диалог создания или редактирования садовода</summary>
    bool CreateOrEditGardener(Gardener gardener);

    bool CreateOrEditParcel(Parcel parcel, IRepository<Gardener> gardenerRepository);
}
