using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Context;
using ReceiptMailing.Data.Entities.Base;
using ReceiptMailing.Services.Interfaces.Repositories;

namespace ReceiptMailing.Data.Repositories
{
    public class DbParcelsRepository<T> : DbRepository<T>, IParcelRepository<T> where T : ParcelEntity, new()
    {
        
        public DbParcelsRepository(ParcelDb db) : base(db) { }

        public async Task<bool> ExistNumber(string number, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(item => item.Number == number, cancel).ConfigureAwait(false);
        }

        public async Task<T> GetByNumber(string number, CancellationToken cancel = default)
        {
            //return await Items.SingleOrDefaultAsync(item => item.Number == Number, Cancel).ConfigureAwait(false);
            return await Items.FirstOrDefaultAsync(item => item.Number == number, cancel).ConfigureAwait(false);
        }

        public async Task<T> DeleteByNumber(string number, CancellationToken cancel = default)
        {
            var item = Set.Local.FirstOrDefault(i => i.Number == number);
            if (item is null)
                item = await Set
                    .Select(i => new T { Id = i.Id, Number = i.Number })
                    .FirstOrDefaultAsync(i => i.Number == number, cancel)
                    .ConfigureAwait(false);
            if (item is null)
                return null;

            return await Delete(item, cancel).ConfigureAwait(false);
        }
    }
}
