using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptMailing.Data.Entities.Base;
using ReceiptMailing.Services.Interfaces.Base;

namespace ReceiptMailing.Data.Entities
{
    public class Gardener : GardenerEntity
    {
        private string? _FirstEmailAddress;
        private string? _SecondEmailAddress;

        /// <summary> Лицевой счет </summary>
        [Required]
        public string Account { get; set; }

        /// <summary> Адрес места жительства </summary>
        public Address? Address { get; set; }
        
        /// <summary> Почтовый адрес </summary>
        public Address? PostAddress { get; set; }
        
        /// <summary> Тип документа </summary>
        public string? Document { get; set; }
        /// <summary> Серия и номер документа </summary>
        public Passport? Passport { get; set; }
        
        /// <summary> Адрес основной электронной почты </summary>
        public string? FirstEmailAddress
        {
            get => _FirstEmailAddress;
            set
            {
                var email = new EmailAddressAttribute();
                if (email.IsValid(value))
                    _FirstEmailAddress = value;
            }
        }

        /// <summary> Адрес дополнительной электронной почты </summary>
        public string? SecondEmailAddress
        {
            get => _SecondEmailAddress;
            set
            {
                var email = new EmailAddressAttribute();
                if (email.IsValid(value))
                    _SecondEmailAddress = value;
            }
        }
    }
}
