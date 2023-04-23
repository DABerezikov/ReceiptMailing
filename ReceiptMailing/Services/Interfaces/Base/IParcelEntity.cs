using System.ComponentModel.DataAnnotations;

namespace ReceiptMailing.Services.Interfaces.Base;

public interface IParcelEntity : IEntity
{
    [Required]
    string Number { get; }
}