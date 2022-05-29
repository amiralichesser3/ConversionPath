﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionPath.Shared.Dtos.ExchangeRates
{
    public class ExchangeRateDto
    {
        public int Id { get; set; }
        public double Rate { get; set; }
        public string SourceCurrency { get; set; }
        public string DestinationCurrency { get; set; }
        public DateTime DateTime { get; set; }
        
        public ExchangeRateDto Clone()
        {
            return new ExchangeRateDto
            {
                Id = this.Id,
                Rate = this.Rate,
                SourceCurrency = this.SourceCurrency,
                DestinationCurrency = this.DestinationCurrency,
                DateTime = this.DateTime
            };
        }
    }
}
