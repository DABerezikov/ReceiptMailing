using System.Threading;
using System.Threading.Tasks;

namespace ReceiptMailing.Services;

public interface IMailService
{
    Task<bool> SendAsync(MailData mailData, CancellationToken ct);
}