using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionPath.Domain.ExchangeRate.Entities
{
    public class ExchangeRate: BaseEntity
    {
        public double Rate { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public string Source { get; set; }
        public DateTime DateTime { get; set; }
    }
}
