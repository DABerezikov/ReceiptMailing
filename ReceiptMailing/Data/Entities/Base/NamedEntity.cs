using ReceiptMailing.Services.Interfaces.Base;

namespace ReceiptMailing.Data.Entities.Base;

public abstract class NamedEntity : Entity, INamedEntity
{
    public string? Name { get; set; }
}