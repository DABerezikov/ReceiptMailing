using System.Collections.Generic;

namespace ReceiptMailing.Services;

public class MailData(
    List<string> to,
    string subject,
    string? body = null,
    List<string>? attachment = null,
    string? from = null,
    string? displayName = null,
    string? replyTo = null,
    string? replyToName = null,
    List<string>? bcc = null,
    List<string>? cc = null)
{
    // Receiver
    
    /// <summary>
    /// To: (кому) — основной получатель письма
    /// </summary>
    public List<string> To { get; } = to;

    /// <summary>
    /// Bcc: (скрытая копия, blind carbon copy) — скрытые получатели письма, чьи адреса не показываются другим получателям.
    /// </summary>
    public List<string> Bcc { get; } = bcc ?? [];

    /// <summary>
    /// Cc: (копия, carbon copy) — вторичные получатели письма, которым направляется копия. Они видят и знают о наличии друг друга.
    /// </summary>
    public List<string> Cc { get; } = cc ?? [];

    // Sender
    
    /// <summary>
    /// From: (от кого) — отправитель письма
    /// </summary>
    public string? From { get; } = from;

    /// <summary>
    /// DisplayName: (от кого) — отображаемое имя отправителя
    /// </summary>
    public string? DisplayName { get; } = displayName;

    /// <summary>
    /// ReplyTo: (переслано от кого) - адрес исходного отправителя
    /// </summary>
    public string? ReplyTo { get; } = replyTo;

    /// <summary>
    /// ReplyToName: (переслано от кого) - имя исходного отправителя
    /// </summary>
    public string? ReplyToName { get; } = replyToName;

    // Content
    public string Subject { get; } = subject;

    public string? Body { get; } = body;

    public List<string>? Attachment { get; } = attachment;

    // Receiver
    // Sender
    // Content
}