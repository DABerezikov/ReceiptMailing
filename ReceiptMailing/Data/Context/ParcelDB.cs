using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Entities;

namespace ReceiptMailing.Data.Context;

public class ParcelDb(DbContextOptions<ParcelDb> options) : DbContext(options)
{
    public DbSet<Parcel> Parcels { get; set; }
    public DbSet<Gardener> Gardeners { get; set; }
}
