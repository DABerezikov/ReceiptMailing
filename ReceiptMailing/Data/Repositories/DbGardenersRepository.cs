using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Context;
using ReceiptMailing.Data.Entities.Base;
using ReceiptMailing.Services.Interfaces.Repositories;

namespace ReceiptMailing.Data.Repositories;

public class DbGardenersRepository<T>(ParcelDb db) : DbRepository<T>(db), IGardenerRepository<T>
    where T : GardenerEntity, new()
{
    protected override IQueryable<T> Items => Set
        .Include("Address")
        .Include("PostAddress")
        .Include("Passport")
        .Include("Parcels");

    public async Task<bool> ExistSurName(string surName, CancellationToken cancel = default) => 
        await Items.AnyAsync(item => item.SurName == surName, cancel).ConfigureAwait(false);

    public async Task<T?> GetBySurName(string surName, CancellationToken cancel = default) => 
        await Items.FirstOrDefaultAsync(item => item.SurName == surName, cancel).ConfigureAwait(false);

    public async Task<T?> DeleteBySurName(string surName, CancellationToken cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.SurName == surName) 
                   ?? await Set
                       .Select(i => new T { Id = i.Id, SurName = i.SurName })
                       .FirstOrDefaultAsync(i => i.SurName == surName, cancel)
                       .ConfigureAwait(false);
        
        return item is not null 
            ? await Delete(item, cancel).ConfigureAwait(false) 
            : null;
    }

    public async Task<bool> ExistPatronymic(string patronymic, CancellationToken cancel = default) => 
        await Items.AnyAsync(item => item.Patronymic == patronymic, cancel).ConfigureAwait(false);

    public async Task<T?> GetByPatronymic(string patronymic, CancellationToken cancel = default) => 
        await Items.FirstOrDefaultAsync(item => item.Patronymic == patronymic, cancel).ConfigureAwait(false);

    public async Task<T?> DeleteByPatronymic(string patronymic, CancellationToken cancel = default)
    {
        var item = Set.Local.FirstOrDefault(i => i.Patronymic == patronymic) 
                   ?? await Set
                       .Select(i => new T { Id = i.Id, Patronymic = i.Patronymic })
                       .FirstOrDefaultAsync(i => i.Patronymic == patronymic, cancel)
                       .ConfigureAwait(false);

        return
            item is not null 
                ? await Delete(item, cancel).ConfigureAwait(false) 
                : null;
    }
}
