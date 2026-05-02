using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Context;
using ReceiptMailing.Data.Entities.Base;
using ReceiptMailing.Services.Interfaces.Repositories;

namespace ReceiptMailing.Data.Repositories;

public class DbRepository<T> : IRepository<T> where T : Entity, new()
{
    private readonly ParcelDb _db;

    protected DbSet<T> Set { get; }

    protected virtual IQueryable<T> Items => Set;

    public bool AutoSaveChanges { get; set; } = true;

    public DbRepository(ParcelDb db)
    {
        _db = db;
        Set = _db.Set<T>();
    }

    public async Task<bool> ExistId(int id, CancellationToken cancel = default)
    {
        return await Items.AnyAsync(item => item.Id == id, cancel).ConfigureAwait(false);
    }

    public async Task<bool> Exist(T item, CancellationToken cancel = default)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));

        return await Items.AnyAsync(i => i.Id == item.Id, cancel).ConfigureAwait(false);
    }

    public async Task<int> GetCount(CancellationToken cancel = default)
    {
        return await Items.CountAsync(cancel).ConfigureAwait(false);
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancel = default)
    {
        return await Items.ToArrayAsync(cancel).ConfigureAwait(false);
    }

    public async Task<IEnumerable<T>> Get(int skip, int count, CancellationToken cancel = default)
    {
        if (count <= 0)
            return [];

        IQueryable<T> query = Items switch
        {
            IOrderedQueryable<T> orderedQuery => orderedQuery,
            { } q => q.OrderBy(i => i.Id)
        };

        if (skip > 0)
            query = query.Skip(skip);

        return await query.Take(count).ToArrayAsync(cancel).ConfigureAwait(false);
    }

    protected record Page(IEnumerable<T> Items, int TotalCount, int PageIndex, int PageSize) : IPage<T>
    {
        public int TotalPagesCount => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public async Task<IPage<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
    {
        if (pageSize <= 0) return new Page([], pageSize, pageIndex, pageSize);

        var query = Items;
        var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);
        if (totalCount == 0)
            return new Page([], 0, pageIndex, pageSize);

        if (query is not IOrderedQueryable<T>)
            query = query.OrderBy(item => item.Id);

        if (pageIndex > 0)
            query = query.Skip(pageIndex * pageSize);
        query = query.Take(pageSize);

        var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

        return new Page(items, totalCount, pageIndex, pageSize);
    }

    public async Task<T?> GetById(int id, CancellationToken cancel = default) =>
        Items switch
        {
            DbSet<T> set => await set.FindAsync([id], cancel).ConfigureAwait(false),
            { } items => await items.FirstOrDefaultAsync(item => item.Id == id, cancel).ConfigureAwait(false),
            _ => throw new InvalidOperationException("Ошибка в определении источника данных")
        };

    public async Task<T> Add(T item, CancellationToken cancel = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        await _db.AddAsync(item, cancel).ConfigureAwait(false);

        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return item;
    }

    public async Task<T> Update(T item, CancellationToken cancel = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        _db.Update(item);

        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return item;
    }

    public async Task<T?> Delete(T item, CancellationToken cancel = default)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (!await ExistId(item.Id, cancel))
            return null;

        _db.Remove(item);

        if (AutoSaveChanges)
            await SaveChanges(cancel).ConfigureAwait(false);

        return item;
    }

    public async Task<T?> DeleteById(int id, CancellationToken cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.Id == id) ?? await Set
            .Select(i => new T { Id = i.Id })
            .FirstOrDefaultAsync(i => i.Id == id, cancel)
            .ConfigureAwait(false);

        return item is not null 
            ? await Delete(item, cancel).ConfigureAwait(false) 
            : null;
    }

    public async Task<int> SaveChanges(CancellationToken cancel = default) => 
        await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
}
