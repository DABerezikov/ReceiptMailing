namespace ReceiptMailing.Data.Entities.Base
{
    public class Passport : Entity
    {
        private string? _series;

        public string? Series
        {
            get => "** **";
            set => _series = value;
        }

        private string? _number;

        public string? Number
        {
            get => "******";
            set => _number = value;
        }

        public override string ToString()
        {
            return $"{Series} {Number}";
        }

        public string? GetPassportSeries() => _series;
        public string? GetPassportNumber() => _number;

        public Passport() { }

        public Passport(string? series, string? number)
        {
            _series = series;
            _number = number;
        }

    }
}
