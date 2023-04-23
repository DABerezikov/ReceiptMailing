using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Context;

namespace ReceiptMailing.Data
{
    public class ParcelDBInitializer
    {
        private readonly ParcelDB _db;

        public ParcelDBInitializer(ParcelDB db) => _db = db;

        public void Initialize()
        {
            _db.Database.Migrate();
            if (_db.Parcels.Any()) return;

        }
        
    }
}
