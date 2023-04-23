using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Entities;

namespace ReceiptMailing.Data.Context
{
    public class ParcelDb : DbContext
    {
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Gardener> Gardeners { get; set; }

        public ParcelDb(DbContextOptions<ParcelDb> options) : base(options) { }
    }
}
