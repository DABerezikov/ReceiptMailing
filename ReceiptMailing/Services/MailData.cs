using System.Collections.Generic;

namespace ReceiptMailing.Services;

public class MailData
{
    // Receiver
    
    /// <summary>
    /// To: (кому) — основной получатель письма
    /// </summary>
    public List<string> To { get; }

    /// <summary>
    /// Bcc: (скрытая копия, blind carbon copy) — скрытые получатели письма, чьи адреса не показываются другим получателям.
    /// </summary>
    public List<string> Bcc { get; }

    /// <summary>
    /// Cc: (копия, carbon copy) — вторичные получатели письма, которым направляется копия. Они видят и знают о наличии друг друга.
    /// </summary>
    public List<string> Cc { get; }

    // Sender
    
    /// <summary>
    /// From: (от кого) — отправитель письма
    /// </summary>
    public string? From { get; }

    /// <summary>
    /// DisplayName: (от кого) — отображаемое имя отправителя
    /// </summary>
    public string? DisplayName { get; }

    /// <summary>
    /// ReplyTo: (переслано от кого) - адрес исходного отправителя
    /// </summary>
    public string? ReplyTo { get; }

    /// <summary>
    /// ReplyToName: (переслано от кого) - имя исходного отправителя
    /// </summary>
    public string? ReplyToName { get; }

    // Content
    public string Subject { get; }

    public string? Body { get; }

    public List<string>? Attachment { get; }

    public MailData(List<string> to, string subject, string? body = null, List<string>? attachment = null, string? from = null, string? displayName = null, string? replyTo = null, string? replyToName = null, List<string>? bcc = null, List<string>? cc = null)
    {
        // Receiver
        To = to;
        Bcc = bcc ?? new List<string>();
        Cc = cc ?? new List<string>();

        // Sender
        From = from;
        DisplayName = displayName;
        ReplyTo = replyTo;
        ReplyToName = replyToName;

        // Content
        Subject = subject;
        Body = body;
        Attachment = attachment;
    }
}