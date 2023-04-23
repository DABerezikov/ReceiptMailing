using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptMailing.Data.Entities.Base;

namespace ReceiptMailing.Data.Entities
{
    public class Parcel: ParcelEntity
    {
        public Gardener Gardener { get; set; }
        public string Street { get; set; }
    }
}
