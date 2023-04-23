using ReceiptMailing.Data.Entities.Base;

namespace ReceiptMailing.Data.Entities
{
    public class Parcel: ParcelEntity
    {
        /// <summary> Владелец участка </summary>
        public Gardener Gardener { get; set; }

        /// <summary> Улица </summary>
        public string Street { get; set; }
        
        /// <summary> Площадь участка, сот </summary>
        public double PlotArea { get; set; }

        /// <summary> Кадастровый номер участка </summary>
        public string? CadastralNumber { get; set; }

        /// <summary> Реквизиты правоустанавливающего документа на участок </summary>
        public string? Details { get; set; }

        /// <summary> Наличие электричества на участке </summary>
        public bool Electrification { get; set; }

        /// <summary> Наличие дома на участке</summary>
        public bool HavingHouse { get; set; }

        /// <summary> Номер дома по адресации СНТ </summary>
        public string? HouseNumber { get; set; }

        /// <summary> Категория участка</summary>
        public string? Category { get; set; }

        /// <summary> Статус участка</summary>
        public string? Status { get; set; }
    }
}
