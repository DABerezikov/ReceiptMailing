namespace ReceiptMailing.Services.Interfaces.Base;

public interface INamedEntity : IEntity
{
   
    string Name { get; }
}