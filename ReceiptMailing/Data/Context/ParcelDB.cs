using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Entities;

namespace ReceiptMailing.Data.Context;

public class ParcelDb(DbContextOptions<ParcelDb> options) : DbContext(options)
{
    public DbSet<Parcel> Parcels { get; set; }
    public DbSet<Gardener> Gardeners { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Parcel>()
            .HasOne(p => p.Gardener)
            .WithMany(g => g.Parcels)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
