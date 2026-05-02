using System.Linq;
using Microsoft.EntityFrameworkCore;
using ReceiptMailing.Data.Context;

namespace ReceiptMailing.Data;

public class ParcelDbInitializer(ParcelDb db)
{
    public void Initialize()
    {
        db.Database.Migrate();
        if (db.Parcels.Any()) return;
    }
}
