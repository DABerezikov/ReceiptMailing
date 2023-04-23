using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Context;
using ReceiptMailing.Data.Entities.Base;
using ReceiptMailing.Services.Interfaces.Repositories;

namespace ReceiptMailing.Data.Repositories;

public class DbGardenersRepository<T> : DbRepository<T>, IGardenerRepository<T> where T : GardenerEntity, new()
{

    public DbGardenersRepository(ParcelDb db) : base(db) { }

    public async Task<bool> ExistSurName(string surName, CancellationToken cancel = default)
    {
        return await Items.AnyAsync(item => item.SurName == surName, cancel).ConfigureAwait(false);
    }

    public async Task<T> GetBySurName(string surName, CancellationToken cancel = default)
    {
        //return await Items.SingleOrDefaultAsync(item => item.SurName == SurName, Cancel).ConfigureAwait(false);
        return await Items.FirstOrDefaultAsync(item => item.SurName == surName, cancel).ConfigureAwait(false);
    }

    public async Task<T> DeleteBySurName(string surName, CancellationToken cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.SurName == surName);
        if (item is null)
            item = await Set
                .Select(i => new T { Id = i.Id, SurName = i.SurName })
                .FirstOrDefaultAsync(i => i.SurName == surName, cancel)
                .ConfigureAwait(false);
        if (item is null)
            return null;

        return await Delete(item, cancel).ConfigureAwait(false);
    }

    public async Task<bool> ExistPatronymic(string patronymic, CancellationToken cancel = default)
    {
        return await Items.AnyAsync(item => item.Patronymic == patronymic, cancel).ConfigureAwait(false);
    }

    public async Task<T> GetByPatronymic(string patronymic, CancellationToken cancel = default)
    {
        //return await Items.SingleOrDefaultAsync(item => item.Patronymic == Patronymic, Cancel).ConfigureAwait(false);
        return await Items.FirstOrDefaultAsync(item => item.Patronymic == patronymic, cancel).ConfigureAwait(false);
    }

    public async Task<T> DeleteByPatronymic(string patronymic, CancellationToken cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.Patronymic == patronymic);
        if (item is null)
            item = await Set
                .Select(i => new T { Id = i.Id, Patronymic = i.Patronymic })
                .FirstOrDefaultAsync(i => i.Patronymic == patronymic, cancel)
                .ConfigureAwait(false);
        if (item is null)
            return null;

        return await Delete(item, cancel).ConfigureAwait(false);
    }
}