namespace ReceiptMailing.Services.Interfaces.Base;

public interface IGardenerEntity : INamedEntity
{
    string SurName { get; }
    string? Patronymic { get; }

}