using System.Threading.Tasks;
using System.Threading;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Linq;
using System;
using System.Diagnostics;
using System.IO;
using MailKit.Net.Smtp;


namespace ReceiptMailing.Services;

public class MailService : IMailService
{
    private readonly MailSettings _settings;

    public MailService(IOptions<MailSettings> settings)
    {
        _settings = settings.Value;
    }
    

    public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
    {
        try
        {
            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver
            // Sender
            mail.From.Add(new MailboxAddress(_settings.DisplayName, mailData.From ?? _settings.From));
            mail.Sender = new MailboxAddress(mailData.DisplayName ?? _settings.DisplayName, mailData.From ?? _settings.From);

            // Receiver
            foreach (string mailAddress in mailData.To)
                mail.To.Add(MailboxAddress.Parse(mailAddress));

            // Set Reply to if specified in mail data
            if (!string.IsNullOrEmpty(mailData.ReplyTo))
                mail.ReplyTo.Add(new MailboxAddress(mailData.ReplyToName, mailData.ReplyTo));

            // BCC
            // Check if a BCC was supplied in the request
            if (mailData.Bcc != null)
            {
                // Get only addresses where value is not null or with whitespace. x = value of address
                foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }

            // CC
            // Check if a CC address was supplied in the request
            if (mailData.Cc != null)
            {
                foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
                    mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            }
            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = mailData.Subject;
            body.HtmlBody = mailData.Body;
            var builder = new BodyBuilder();
            if (mailData.Attachment != null)
                foreach (var filePath in mailData.Attachment)
                {
                    var buffer = File.ReadAllBytes(filePath);
                    
                    builder.Attachments.Add(Path.GetFileName(filePath), buffer, new ContentType("AdobePDF", "pdf"));
                   
                }
            mail.Body = builder.ToMessageBody();

            #endregion

            #region Send Mail

            using var smtp = new SmtpClient();

            smtp.Timeout = (int)TimeSpan.FromSeconds(5).TotalMilliseconds;

            if (_settings.UseSsl)
            {
                Debug.WriteLine(smtp.IsConnected);
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct).ConfigureAwait(false);
                Debug.WriteLine(smtp.IsConnected);
            }
            else if (_settings.UseStartTls)
            {
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
            }
            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
            await smtp.SendAsync(mail, ct);
            await smtp.DisconnectAsync(true, ct);

            #endregion

            return true;

        }
        catch (Exception)
        {
            return false;
        }
    }
    
}