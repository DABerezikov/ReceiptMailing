using ReceiptMailing.Services.Interfaces.Base;

namespace ReceiptMailing.Data.Entities.Base
{
    public abstract class Entity : IEntity
    {
        public int Id { get; set; }
    }
}
