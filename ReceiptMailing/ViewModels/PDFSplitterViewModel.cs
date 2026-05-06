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

    #region Прогресс разделения

    public int SplitProgress
    {
        get;
        set => Set(ref field, value);
    } = 0;

    public bool IsSplitting
    {
        get;
        set => Set(ref field, value);
    } = false;

    public string SplitProgressText
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region Прогресс рассылки

    public int SendProgress
    {
        get;
        set => Set(ref field, value);
    } = 0;

    public bool IsSending
    {
        get;
        set => Set(ref field, value);
    } = false;

    public string SendProgressText
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

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
        ??= new LambdaCommandAsync(OnSplitPDFCommandExecuted, CanSplitPdfCommandExecute);

    /// <summary> Проверка возможности выполнения - Команда разделения файла квитанций </summary>
    private bool CanSplitPdfCommandExecute() => PdfFilePath != string.Empty;

    /// <summary> Логика выполнения - Команда разделения файла квитанций </summary>
    private async Task OnSplitPDFCommandExecuted()
    {
        splitter.Path = PdfFilePath;
        IsSplitting = true;
        SplitProgress = 0;
        SplitProgressText = "0%";
        Status = "Разделение квитанций...";

        var progress = new Progress<int>(v =>
        {
            SplitProgress = v;
            SplitProgressText = $"{v}%";
        });

        var result = await Task.Run(() => splitter.PdfSplit(progress));

        IsSplitting = false;
        SplitProgress = 0;
        SplitProgressText = string.Empty;
        Status = "Готов!";
        SplitFilePath = splitter.FileFolderPath;
        userDialog.Information(result, "Обрезка квитанций");
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
        int total = listFiles.Count;
        int countSendFile = 0;
        int processed = 0;

        IsSending = true;
        SendProgress = 0;
        SendProgressText = $"0 из {total}";
        Status = $"Отправка: 0 из {total}";

        foreach (var filePath in listFiles)
        {
            if (!await SendReceipt(filePath))
            {
                ListNotSendReceipts.Add(filePath);
            }
            else
            {
                countSendFile++;
            }

            processed++;
            SendProgress = total > 0 ? processed * 100 / total : 0;
            SendProgressText = $"{processed} из {total}";
            Status = $"Отправка: {processed} из {total}";
        }

        IsSending = false;
        SendProgress = 0;
        SendProgressText = string.Empty;
        Status = "Готов!";

        SaveListFileNotSend();
        userDialog.Information($"Отправлено {countSendFile} из {total}", "Почтальон");
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
