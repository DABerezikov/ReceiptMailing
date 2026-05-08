using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ReceiptMailing.Data.Entities.Base;

namespace ReceiptMailing.Data.Entities;

public class Gardener : GardenerEntity
{
    /// <summary> Лицевой счет </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary> Адрес места жительства </summary>
        public Address Address { get; set; } = new();

        /// <summary> Почтовый адрес </summary>
        public Address PostAddress { get; set; } = new();

        /// <summary> Номер телефона </summary>
        public string? PhoneNumber { get; set; }
        
        /// <summary> Документ о приеме в члены СНТ </summary>
        public string? Document { get; set; }
        /// <summary> Серия и номер паспорта </summary>
        public Passport Passport { get; set; } = new();

        /// <summary> Адрес основной электронной почты </summary>
        public string? FirstEmailAddress
        {
            get;
            set
            {
                var email = new EmailAddressAttribute();
                if (email.IsValid(value) || value == string.Empty)
                    field = value;
            }
        }

        /// <summary> Адрес дополнительной электронной почты </summary>
        public string? SecondEmailAddress
        {
            get;
            set
            {
                var email = new EmailAddressAttribute();
                if (email.IsValid(value) || value == string.Empty)
                    field = value;
            }
        }

        public override string ToString()
        {
            var name = string.Join(" ", new[] { SurName, Name, Patronymic }.Where(s => !string.IsNullOrEmpty(s)));
            return string.IsNullOrWhiteSpace(Account)
                ? name
                : $"{name} (сч. {Account})";
        }

        public ICollection<Parcel> Parcels { get; set; } = new HashSet<Parcel>();
}
