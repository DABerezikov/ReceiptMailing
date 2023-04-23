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
        
        public DbParcelsRepository(ParcelDB db) : base(db) { }

        public async Task<bool> ExistNumber(string Number, CancellationToken Cancel = default)
        {
            return await Items.AnyAsync(item => item.Number == Number, Cancel).ConfigureAwait(false);
        }

        public async Task<T> GetByNumber(string Number, CancellationToken Cancel = default)
        {
            //return await Items.SingleOrDefaultAsync(item => item.Number == Number, Cancel).ConfigureAwait(false);
            return await Items.FirstOrDefaultAsync(item => item.Number == Number, Cancel).ConfigureAwait(false);
        }

        public async Task<T> DeleteByNumber(string Number, CancellationToken Cancel = default)
        {
            var item = Set.Local.FirstOrDefault(i => i.Number == Number);
            if (item is null)
                item = await Set
                    .Select(i => new T { Id = i.Id, Number = i.Number })
                    .FirstOrDefaultAsync(i => i.Number == Number, Cancel)
                    .ConfigureAwait(false);
            if (item is null)
                return null;

            return await Delete(item, Cancel).ConfigureAwait(false);
        }
    }
}
