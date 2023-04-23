using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Entities;

namespace ReceiptMailing.Data.Context
{
    public class ParcelDB : DbContext
    {
        public DbSet<Parcel> Parcels { get; set; }

        public ParcelDB(DbContextOptions<ParcelDB> options) : base(options) { }
    }
}
