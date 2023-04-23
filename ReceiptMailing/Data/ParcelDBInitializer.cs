using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Context;

namespace ReceiptMailing.Data
{
    public class ParcelDbInitializer
    {
        private readonly ParcelDb _db;

        public ParcelDbInitializer(ParcelDb db) => _db = db;

        public void Initialize()
        {
            _db.Database.Migrate();
            if (_db.Parcels.Any()) return;

        }
        
    }
}
