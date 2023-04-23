using System.Threading;
using System.Threading.Tasks;
using ReceiptMailing.Services.Interfaces.Base;

namespace ReceiptMailing.Services.Interfaces.Repositories;

public interface IGardenerRepository<T> : IRepository<T> where T : IGardenerEntity
{
    Task<bool> ExistSurName(string surName, CancellationToken cancel = default);

    Task<T> GetBySurName(string surName, CancellationToken cancel = default);

    Task<T> DeleteBySurName(string surName, CancellationToken cancel = default);

    Task<bool> ExistPatronymic(string patronymic, CancellationToken cancel = default);

    Task<T> GetByPatronymic(string patronymic, CancellationToken cancel = default);

    Task<T> DeleteByPatronymic(string patronymic, CancellationToken cancel = default);
}