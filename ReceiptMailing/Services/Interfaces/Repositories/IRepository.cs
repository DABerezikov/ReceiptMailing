using ReceiptMailing.Services.Interfaces.Base;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReceiptMailing.Data.Entities;

namespace ReceiptMailing.Services.Interfaces.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<bool> ExistId(int id, CancellationToken cancel = default);
        //async Task<bool> ExistId(int Id, CancellationToken Cancel = default) => await GetById(Id, Cancel) is not null;

        Task<bool> Exist(T item, CancellationToken cancel = default);

        Task<int> GetCount(CancellationToken cancel = default);

        Task<IEnumerable<T>> GetAll(CancellationToken cancel = default);

        Task<IEnumerable<T>> Get(int skip, int count, CancellationToken cancel = default);

        Task<IPage<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default);

        //async Task<T> GetById(int Id, CancellationToken Cancel = default) => (await GetAll(Cancel)).FirstOrDefault(item => item.Id == Id);
        Task<T> GetById(int id, CancellationToken cancel = default);

        Task<T> Add(T item, CancellationToken cancel = default);

        Task<T> Update(T item, CancellationToken cancel = default);

        Task<T> Delete(T item, CancellationToken cancel = default);

        Task<T> DeleteById(int id, CancellationToken cancel = default);
       
    }

    public interface IPage<out T>
    {
        IEnumerable<T> Items { get; }

        int TotalCount { get; }

        int PageIndex { get; }

        int PageSize { get; }

        int TotalPagesCount { get; }
        //int TotalPagesCount => (int) Math.Ceiling((double) TotalCount / PageSize);
    }
}
