namespace ReceiptMailing.Data.Entities.Base
{
    public class Address : Entity
    {
        /// <summary> Почтовый индекс </summary>
        public string? PostalCode { get; set; }
        
        /// <summary> Область </summary>
        public string? Province { get; set; }
        
        /// <summary> Округ </summary>
        public string? Region { get; set; }
        /// <summary> Населенный пункт </summary>
        public string? City { get; set; }
        /// <summary> Улица </summary>
        public string? Street { get; set; }
        /// <summary> Дом </summary>
        public string? House { get; set; }
        /// <summary> Корпус </summary>
        public string? Building { get; set; }
        /// <summary> Квартира </summary>
        public string? Room { get; set; }
       
        


    }
}
