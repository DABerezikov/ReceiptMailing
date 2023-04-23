using ReceiptMailing.Services.Interfaces.Base;
using System.Threading;
using System.Threading.Tasks;

namespace ReceiptMailing.Services.Interfaces.Repositories
{
    public interface IParcelRepository<T> : IRepository<T> where T : IParcelEntity
    {
        Task<bool> ExistNumber(string number, CancellationToken cancel = default);

        Task<T> GetByNumber(string number, CancellationToken cancel = default);

        Task<T> DeleteByNumber(string number, CancellationToken cancel = default);
    }
}
