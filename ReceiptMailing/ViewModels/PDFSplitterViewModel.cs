using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels;

internal class PdfSplitterViewModel(
    IUserDialog userDialog,
    ReceiptsSplitter splitter,
    IMailService email,
    IParcelRepository<Parcel> parcel)
    : ViewModel
{
    #region Title : string - Заголовок окна

    /// <summary>Заголовок окна</summary>
    public string Title
    {
        get;
        set => Set(ref field, value);
    } = "Биоробот Константин";

    #endregion

    #region Status : string - Статус

    /// <summary>Статус</summary>
    public string Status
    {
        get;
        set => Set(ref field, value);
    } = "Готов!";

    #endregion

    #region PDFFilePath : string - Путь к файлу с квитанциями

    /// <summary>Путь к файлу с квитанциями</summary>
    public string? PdfFilePath
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region SplitFilePath : string - Путь к папке с квитанциями

    /// <summary>Путь к файлу с квитанциями</summary>
    public string? SplitFilePath
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region ListNotSendReceipts : List<string> - Список неотправленных файлов

    /// <summary>Список неотправленных файлов</summary>
    public List<string> ListNotSendReceipts
    {
        get;
        set => Set(ref field, value);
    } = new();

    #endregion

    #region Command OpenPDFCommand - команда для открытия файла с квитанциями

    /// <summary> команда для открытия файла с квитанциями </summary>
    public ICommand OpenPdfCommand => field
        ??= new LambdaCommand(OnOpenPDFCommandExecuted, CanOpenPdfCommandExecute);

    /// <summary> Проверка возможности выполнения - команда для открытия файла с квитанциями </summary>
    private bool CanOpenPdfCommandExecute() => true;

    /// <summary> Логика выполнения - команда для открытия файла с квитанциями </summary>
    private void OnOpenPDFCommandExecuted()
    {
        var temp = userDialog.OpenFile("Выбор исходного файла с квитанциями");
        if (temp is null) return;
        PdfFilePath = temp.DirectoryName + "\\" + temp.Name;
    }

    #endregion

    #region Command SplitPDFCommand - Команда разделения файла квитанций

    /// <summary> Команда разделения файла квитанций </summary>
    public ICommand SplitPdfCommand => field
        ??= new LambdaCommand(OnSplitPDFCommandExecuted, CanSplitPdfCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда разделения файла квитанций </summary>
    private bool CanSplitPdfCommandExecute() => PdfFilePath != string.Empty;

    /// <summary> Логика выполнения - Команда разделения файла квитанций </summary>
    private void OnSplitPDFCommandExecuted()
    {
        splitter.Path = PdfFilePath;
        userDialog.Information(splitter.PdfSplit(), "Обрезка квитанций");
        SplitFilePath = splitter.FileFolderPath;
    }

    #endregion

    #region Command SendReceiptCommand - Команда разделения файла квитанций

    /// <summary> Команда разделения файла квитанций </summary>
    public ICommand SendReceiptCommand => field
        ??= new LambdaCommandAsync(OnSendReceiptCommandExecuted, CanSendReceiptCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда разделения файла квитанций </summary>
    private bool CanSendReceiptCommandExecute() => SplitFilePath != string.Empty;

    /// <summary> Логика выполнения - Команда разделения файла квитанций </summary>
    private async Task OnSendReceiptCommandExecuted()
    {
        if (string.IsNullOrEmpty(SplitFilePath)) return;
        var listFiles = new List<string>(Directory.EnumerateFiles(SplitFilePath));
        int countSendFile = 0;

        foreach (var filePath in listFiles)
        {
            if (!await SendReceipt(filePath))
            {
                ListNotSendReceipts.Add(filePath);
                continue;
            }
            countSendFile++;
        }

        SaveListFileNotSend();
        userDialog.Information($"Отправлено {countSendFile} из {listFiles.Count}", "Почтальон");
    }

    #endregion

    private async Task<(string?, string?)> GetEmailCurrentParcel(string filePath)
    {
        var indexStart = filePath.LastIndexOf(" ") + 1;
        var length = filePath.LastIndexOf(".") - indexStart;
        var parcelNumber = filePath.Substring(indexStart, length);
        parcelNumber = parcelNumber.Replace('_', '/');
        var currentParcel = await parcel.GetByNumber(parcelNumber);
        if (currentParcel == null) return (null, null);
        return (currentParcel.Gardener.FirstEmailAddress, currentParcel.Gardener.SecondEmailAddress);
    }

    private async Task<bool> SendReceipt(string filePath)
    {
        var listTo = new List<string>();
        var email1 = await GetEmailCurrentParcel(filePath).ConfigureAwait(false);
        if (string.IsNullOrEmpty(email1.Item1)) return false;
        listTo.Add(email1.Item1);
        if (email1.Item2 != null)
            listTo.Add(email1.Item2);
        var attachment = new List<string> { filePath };
        var ct = CancellationToken.None;
        var msg = new MailData(listTo, "Квитанция СНТ \"Тимирязевец\"",
            "C уважением, \nПравление СНТ \"Тимирязевец\"", attachment);
        return await email.SendAsync(msg, ct);
    }

    private void SaveListFileNotSend()
    {
        string file = @$"{SplitFilePath}\NotSend.csv";
        string separator = ",";
        StringBuilder output = new StringBuilder();
        foreach (var notSend in ListNotSendReceipts)
        {
            output.AppendLine(string.Join(separator, notSend));
        }

        try
        {
            using var writer = new StreamWriter(file, false, Encoding.UTF8);
            writer.WriteLineAsync(output);
        }
        catch (Exception)
        {
            Console.WriteLine("Data could not be written to the CSV file.");
        }
    }
}
