namespace ReceiptMailing.Data.Entities.Base
{
    public class Passport : Entity
    {
        private string _Series;

        public string Series
        {
            get => "** **";
            set => _Series = value;
        }

        private string _Number;

        public string Number
        {
            get => "******";
            set => _Number = value;
        }

        public override string ToString()
        {
            return $"{Series} {Number}";
        }

        public string GetPassportSeries() => _Series;
        public string GetPassportNumber() => _Number;

        public Passport() { }

        public Passport(string series, string number)
        {
            _Series = series;
            _Number = number;
        }

    }
}
