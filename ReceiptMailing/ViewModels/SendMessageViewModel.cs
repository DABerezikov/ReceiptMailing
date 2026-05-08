using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Options;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services;
using ReceiptMailing.Services.Interfaces;
using ReceiptMailing.Services.Interfaces.Repositories;
using ReceiptMailing.ViewModels.Base;
using System;

namespace ReceiptMailing.ViewModels;

internal class SendMessageViewModel : ViewModel
{
    private readonly IRepository<Gardener> _gardenerRepository;
    private readonly IMailService _mailService;
    private readonly IUserDialog _userDialog;
    private readonly IOptionsMonitor<MailSettings> _mailOptions;

    public event Action<bool>? CloseRequested;

    public SendMessageViewModel(
        IOptionsMonitor<MessageSettings> options,
        IOptionsMonitor<MailSettings> mailOptions,
        IRepository<Gardener> gardenerRepository,
        IMailService mailService,
        IUserDialog userDialog)
    {
        _gardenerRepository = gardenerRepository;
        _mailService = mailService;
        _userDialog = userDialog;
        _mailOptions = mailOptions;

        var s = options.CurrentValue;
        Subject   = s.DefaultSubject   ?? string.Empty;
        Signature = s.DefaultSignature ?? string.Empty;
    }

    public string Title { get; } = "Рассылка сообщений садоводам";

    #region Subject : string - Тема письма

    public string Subject
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region Body : string - Текст сообщения

    public string Body
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region Signature : string - Подпись

    public string Signature
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region IsSending : bool - Идёт отправка

    public bool IsSending
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region SendStatus : string - Статус отправки

    public string SendStatus
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    public ObservableCollection<GardenerSelectionItem> Gardeners { get; } = [];
    public ObservableCollection<string> Attachments { get; } = [];

    #region SelectedAttachment : string? - Выбранное вложение

    public string? SelectedAttachment
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    public int SelectedWithEmailCount => Gardeners.Count(g => g.IsSelected && g.HasEmail);
    public int TotalWithEmailCount    => Gardeners.Count(g => g.HasEmail);

    #region Command LoadCommand - Загрузить список садоводов

    public ICommand LoadCommand => field
        ??= new LambdaCommandAsync(OnLoadCommandExecuted);

    private async Task OnLoadCommandExecuted()
    {
        // Заглушка фильтра — возвращает всех садоводов. Логика фильтрации будет добавлена позднее.
        var all = await _gardenerRepository.GetAll();
        foreach (var g in all)
        {
            var item = new GardenerSelectionItem(g);
            item.PropertyChanged += (_, _) => OnPropertyChanged(nameof(SelectedWithEmailCount));
            Gardeners.Add(item);
        }
        OnPropertyChanged(nameof(TotalWithEmailCount));
        OnPropertyChanged(nameof(SelectedWithEmailCount));
    }

    #endregion

    #region Command SelectAllCommand - Выбрать всех садоводов с email

    public ICommand SelectAllCommand => field
        ??= new LambdaCommand(() =>
        {
            foreach (var g in Gardeners.Where(g => g.HasEmail))
                g.IsSelected = true;
            OnPropertyChanged(nameof(SelectedWithEmailCount));
        });

    #endregion

    #region Command DeselectAllCommand - Снять выделение

    public ICommand DeselectAllCommand => field
        ??= new LambdaCommand(() =>
        {
            foreach (var g in Gardeners)
                g.IsSelected = false;
            OnPropertyChanged(nameof(SelectedWithEmailCount));
        });

    #endregion

    #region Command AddAttachmentCommand - Добавить вложение

    public ICommand AddAttachmentCommand => field
        ??= new LambdaCommand(OnAddAttachmentCommandExecuted);

    private void OnAddAttachmentCommandExecuted()
    {
        var file = _userDialog.OpenFile(
            "Выберите файл для вложения",
            "Все файлы (*.*)|*.*|PDF (*.pdf)|*.pdf|Word (*.docx)|*.docx|Excel (*.xlsx)|*.xlsx");
        if (file is null) return;
        if (!Attachments.Contains(file.FullName))
            Attachments.Add(file.FullName);
    }

    #endregion

    #region Command RemoveAttachmentCommand - Удалить выбранное вложение

    public ICommand RemoveAttachmentCommand => field
        ??= new LambdaCommand(
            () => Attachments.Remove(SelectedAttachment!),
            () => SelectedAttachment is not null);

    #endregion

    #region Command SaveDefaultsCommand - Сохранить тему и подпись как умолчание

    public ICommand SaveDefaultsCommand => field
        ??= new LambdaCommand(OnSaveDefaultsCommandExecuted);

    private void OnSaveDefaultsCommandExecuted()
    {
        var settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
        var json = File.ReadAllText(settingsPath);
        var root = JsonNode.Parse(json)!;

        if (root["MessageSettings"] is null)
            root["MessageSettings"] = new JsonObject();

        var ms = root["MessageSettings"]!;
        ms["DefaultSubject"]   = Subject;
        ms["DefaultSignature"] = Signature;

        File.WriteAllText(settingsPath,
            root.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

        _userDialog.Information(
            "Тема и подпись сохранены как значения по умолчанию.",
            "Сохранение умолчаний");
    }

    #endregion

    #region Command SendCommand - Отправить сообщения

    public ICommand SendCommand => field
        ??= new LambdaCommandAsync(OnSendCommandExecuted, () => !IsSending);

    private async Task OnSendCommandExecuted()
    {
        var recipients = Gardeners.Where(g => g.IsSelected && g.HasEmail).ToList();
        if (recipients.Count == 0)
        {
            _userDialog.Warning("Нет выбранных садоводов с email-адресом.", "Рассылка");
            return;
        }

        IsSending = true;
        var sent   = 0;
        var failed = 0;

        var body = string.IsNullOrWhiteSpace(Signature)
            ? Body
            : $"{Body}\n\n{Signature}";
        var attachments = Attachments.Count > 0 ? [.. Attachments] : (List<string>?)null;
        var delayMs = _mailOptions.CurrentValue.SendDelaySeconds * 1000;

        foreach (var item in recipients)
        {
            var emails = new List<string>();
            if (!string.IsNullOrWhiteSpace(item.Gardener.FirstEmailAddress))
                emails.Add(item.Gardener.FirstEmailAddress!);
            if (!string.IsNullOrWhiteSpace(item.Gardener.SecondEmailAddress))
                emails.Add(item.Gardener.SecondEmailAddress!);

            var mailData = new MailData(emails, Subject, body, attachments);
            var ok = await _mailService.SendAsync(mailData, CancellationToken.None);

            if (ok) sent++; else failed++;
            SendStatus = $"Отправлено: {sent} из {recipients.Count}, ошибок: {failed}";

            if (delayMs > 0 && sent + failed < recipients.Count)
                await Task.Delay(delayMs);
        }

        IsSending = false;
        _userDialog.Information(
            $"Рассылка завершена.\nОтправлено: {sent}, ошибок: {failed}.",
            "Рассылка");
        CloseRequested?.Invoke(true);
    }

    #endregion

    #region Command CancelCommand - Закрыть окно

    public ICommand CancelCommand => field
        ??= new LambdaCommand(() => CloseRequested?.Invoke(false), () => !IsSending);

    #endregion
}
