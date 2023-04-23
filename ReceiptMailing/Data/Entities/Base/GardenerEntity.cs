using ReceiptMailing.Services.Interfaces.Base;

namespace ReceiptMailing.Data.Entities.Base;

public abstract class GardenerEntity : NamedEntity, IGardenerEntity
{
    public string SurName { get; set; }
    public string? Patronymic { get; set; }

}