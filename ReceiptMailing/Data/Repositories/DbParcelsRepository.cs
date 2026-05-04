using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Context;
using ReceiptMailing.Data.Entities;
using ReceiptMailing.Data.Entities.Base;
using ReceiptMailing.Services.Interfaces.Repositories;

namespace ReceiptMailing.Data.Repositories;

public class DbParcelsRepository<T>(ParcelDb db) : DbRepository<T>(db), IParcelRepository<T>
    where T : ParcelEntity, new()
{
    protected override IQueryable<T> Items => Set
        .Include("Gardener")
        .Include("Gardener.Address")
        .Include("Gardener.PostAddress")
        .Include("Gardener.Passport");

    public override async Task<T?> Delete(T item, CancellationToken cancel = default)
    {
        if (item is Parcel { Gardener: not null } parcel)
            _db.Entry(parcel.Gardener).State = EntityState.Unchanged;
        return await base.Delete(item, cancel);
    }

    public async Task<bool> ExistNumber(string number, CancellationToken cancel = default)
    {
        return await Items.AnyAsync(item => item.Number == number, cancel).ConfigureAwait(false);
    }

    public async Task<T?> GetByNumber(string number, CancellationToken cancel = default)
    {
        return await Items.FirstOrDefaultAsync(item => item.Number == number, cancel).ConfigureAwait(false);
    }

    public async Task<T?> DeleteByNumber(string number, CancellationToken cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.Number == number) ?? await Set
            .Select(i => new T { Id = i.Id, Number = i.Number })
            .FirstOrDefaultAsync(i => i.Number == number, cancel)
            .ConfigureAwait(false);

        return item is not null 
            ? await Delete(item, cancel).ConfigureAwait(false) 
            : null;
    }
}
