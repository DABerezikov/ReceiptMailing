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

    public DbGardenersRepository(ParcelDB db) : base(db) { }

    public async Task<bool> ExistSurName(string SurName, CancellationToken Cancel = default)
    {
        return await Items.AnyAsync(item => item.SurName == SurName, Cancel).ConfigureAwait(false);
    }

    public async Task<T> GetBySurName(string SurName, CancellationToken Cancel = default)
    {
        //return await Items.SingleOrDefaultAsync(item => item.SurName == SurName, Cancel).ConfigureAwait(false);
        return await Items.FirstOrDefaultAsync(item => item.SurName == SurName, Cancel).ConfigureAwait(false);
    }

    public async Task<T> DeleteBySurName(string SurName, CancellationToken Cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.SurName == SurName);
        if (item is null)
            item = await Set
                .Select(i => new T { Id = i.Id, SurName = i.SurName })
                .FirstOrDefaultAsync(i => i.SurName == SurName, Cancel)
                .ConfigureAwait(false);
        if (item is null)
            return null;

        return await Delete(item, Cancel).ConfigureAwait(false);
    }

    public async Task<bool> ExistPatronymic(string Patronymic, CancellationToken Cancel = default)
    {
        return await Items.AnyAsync(item => item.Patronymic == Patronymic, Cancel).ConfigureAwait(false);
    }

    public async Task<T> GetByPatronymic(string Patronymic, CancellationToken Cancel = default)
    {
        //return await Items.SingleOrDefaultAsync(item => item.Patronymic == Patronymic, Cancel).ConfigureAwait(false);
        return await Items.FirstOrDefaultAsync(item => item.Patronymic == Patronymic, Cancel).ConfigureAwait(false);
    }

    public async Task<T> DeleteByPatronymic(string Patronymic, CancellationToken Cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.Patronymic == Patronymic);
        if (item is null)
            item = await Set
                .Select(i => new T { Id = i.Id, Patronymic = i.Patronymic })
                .FirstOrDefaultAsync(i => i.Patronymic == Patronymic, Cancel)
                .ConfigureAwait(false);
        if (item is null)
            return null;

        return await Delete(item, Cancel).ConfigureAwait(false);
    }
}