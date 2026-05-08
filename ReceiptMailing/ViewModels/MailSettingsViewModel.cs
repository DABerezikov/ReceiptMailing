using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Windows.Input;
using Microsoft.Extensions.Options;
using ReceiptMailing.Infrastructure.Commands;
using ReceiptMailing.Services;
using ReceiptMailing.ViewModels.Base;

namespace ReceiptMailing.ViewModels;

internal class MailSettingsViewModel : ViewModel
{
    public event Action<bool>? CloseRequested;

    public MailSettingsViewModel(IOptionsMonitor<MailSettings> options)
    {
        var s = options.CurrentValue;
        DisplayName  = s.DisplayName  ?? string.Empty;
        From         = s.From         ?? string.Empty;
        UserName     = s.UserName     ?? string.Empty;
        Password     = s.Password     ?? string.Empty;
        Host         = s.Host         ?? string.Empty;
        Port         = s.Port;
        UseSsl           = s.UseSsl;
        UseStartTls      = s.UseStartTls;
        SendDelaySeconds = s.SendDelaySeconds;
    }

    #region Title : string - Заголовок окна

    public string Title { get; } = "Настройки почты";

    #endregion

    #region DisplayName : string - Имя отправителя

    public string DisplayName
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region From : string - Email отправителя

    public string From
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region UserName : string - Пользователь SMTP

    public string UserName
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region Password : string - Пароль SMTP

    public string Password
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region Host : string - SMTP сервер

    public string Host
    {
        get;
        set => Set(ref field, value);
    } = string.Empty;

    #endregion

    #region Port : int - Порт SMTP

    public int Port
    {
        get;
        set => Set(ref field, value);
    } = 465;

    #endregion

    #region UseSsl : bool - Использовать SSL

    public bool UseSsl
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region UseStartTls : bool - Использовать StartTLS

    public bool UseStartTls
    {
        get;
        set => Set(ref field, value);
    }

    #endregion

    #region SendDelaySeconds : int - Задержка между письмами (секунды)

    public int SendDelaySeconds
    {
        get;
        set => Set(ref field, value);
    } = 2;

    #endregion

    #region IsPasswordVisible : bool - Видимость пароля

    public bool IsPasswordVisible
    {
        get;
        set => Set(ref field, value);
    } = false;

    #endregion

    #region Command TogglePasswordVisibilityCommand - Переключить видимость пароля

    public ICommand TogglePasswordVisibilityCommand => field
        ??= new LambdaCommand(() => IsPasswordVisible = !IsPasswordVisible);

    #endregion

    #region Command SaveCommand - Сохранить настройки

    public ICommand SaveCommand => field
        ??= new LambdaCommand(OnSaveCommandExecuted);

    private void OnSaveCommandExecuted()
    {
        var settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

        var json  = File.ReadAllText(settingsPath);
        var root  = JsonNode.Parse(json)!;
        var ms    = root["MailSettings"]!;

        ms["DisplayName"]  = DisplayName;
        ms["From"]         = From;
        ms["UserName"]     = UserName;
        ms["Password"]     = Password;
        ms["Host"]         = Host;
        ms["Port"]         = Port;
        ms["UseSSL"]           = UseSsl;
        ms["UseStartTls"]      = UseStartTls;
        ms["SendDelaySeconds"] = SendDelaySeconds;

        File.WriteAllText(settingsPath,
            root.ToJsonString(new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

        CloseRequested?.Invoke(true);
    }

    #endregion

    #region Command CancelCommand - Отмена

    public ICommand CancelCommand => field
        ??= new LambdaCommand(() => CloseRequested?.Invoke(false));

    #endregion
}
