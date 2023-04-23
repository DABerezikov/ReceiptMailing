using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptMailing.Data.Entities
{
    public class Passport
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
