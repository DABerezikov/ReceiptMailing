using System.Threading;
using System.Threading.Tasks;
using ReceiptMailing.Services.Interfaces.Base;

namespace ReceiptMailing.Services.Interfaces.Repositories;

public interface IGardenerRepository<T> : IRepository<T> where T : IGardenerEntity
{
    Task<bool> ExistSurName(string SurName, CancellationToken Cancel = default);

    Task<T> GetBySurName(string SurName, CancellationToken Cancel = default);

    Task<T> DeleteBySurName(string SurName, CancellationToken Cancel = default);

    Task<bool> ExistPatronymic(string Patronymic, CancellationToken Cancel = default);

    Task<T> GetByPatronymic(string Patronymic, CancellationToken Cancel = default);

    Task<T> DeleteByPatronymic(string Patronymic, CancellationToken Cancel = default);
}