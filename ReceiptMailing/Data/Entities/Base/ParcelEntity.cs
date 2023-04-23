using System.ComponentModel.DataAnnotations;
using ReceiptMailing.Services.Interfaces.Base;

namespace ReceiptMailing.Data.Entities.Base;

public abstract class ParcelEntity : Entity, IParcelEntity
{
    [Required]
    public string Number { get; set; }
}