using ConversionPath.Domain.Contracts; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionPath.Domain.ExchangeRates.Entities
{
    public class ExchangeRate : BaseEntity, IEquatable<ExchangeRate>, ICloneable, IAggregateRoot
    {
        public double Rate { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; } 
        public DateTime DateTime { get; set; }

        public object Clone()
        {
            return new ExchangeRate
            {
                Id = this.Id,
                Rate = this.Rate,
                SourceCurrency = this.SourceCurrency,
                DestinationCurrency = this.DestinationCurrency,
                DateTime = this.DateTime
            };
        }

        public bool Equals(ExchangeRate? other)
        {
            return this.Id.Equals(other?.Id);
        }

        public override int GetHashCode()
        {
            int hash1 = Rate.GetHashCode();
            int hash2 = SourceCurrency.GetHashCode();
            int hash3 = DestinationCurrency.GetHashCode();
            int hash4 = DateTime.GetHashCode();

            return hash1 ^ hash2 ^ hash3 ^ hash4;
        }
    }
}
