using ReceiptMailing.Services.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReceiptMailing.Services.Interfaces.Repositories
{
    public interface IParcelRepository<T> : IRepository<T> where T : IParcelEntity
    {
        Task<bool> ExistNumber(string Number, CancellationToken Cancel = default);

        Task<T> GetByNumber(string Number, CancellationToken Cancel = default);

        Task<T> DeleteByNumber(string Number, CancellationToken Cancel = default);
    }
}
