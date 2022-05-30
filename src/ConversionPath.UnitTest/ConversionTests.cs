using ConversionPath.Application.Conversion;
using ConversionPath.Application.ExchangeRates.Queries;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Shared.Dtos.ExchangeRates;
using FakeItEasy;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConversionPath.UnitTest
{
    public class ConversionTests
    {
        ICollection<ExchangeRateDto> seedData = new List<ExchangeRateDto>()
        {
            new ExchangeRateDto { SourceCurrency = "USD", DestinationCurrency = "IRR", DateTime = DateTime.Now, Rate = 42400 },
            new ExchangeRateDto { SourceCurrency = "EUR", DestinationCurrency = "USD", DateTime = DateTime.Now, Rate = 1.07 },
            new ExchangeRateDto {SourceCurrency = "ETH", DestinationCurrency = "EUR", DateTime = DateTime.Now, Rate = 1671725 },
            new ExchangeRateDto {SourceCurrency = "GBP", DestinationCurrency = "USD", DateTime = DateTime.Now, Rate = 1.26 },
            new ExchangeRateDto { SourceCurrency = "ETH", DestinationCurrency = "BNB", DateTime = DateTime.Now, Rate = 0.1688 },
            new ExchangeRateDto { SourceCurrency = "USDT", DestinationCurrency = "TRX", DateTime = DateTime.Now, Rate = 0.3680},

        }; 

        [Theory]
        [InlineData("usd", "irr", 1000)]
        [InlineData("IRR", "USD", 1000)]
        [InlineData("EUR", "IRR", 1000)]
        [InlineData("GBP", "irr", 1000)]
        [InlineData("USD", "USD", 1000)]
        [InlineData("BNB", "irr", 1000)]
        public async Task ShouldConvert(string source, string destination, double amount)
        { 
            var converter = new CurrencyConverter();
            converter.SetRates(seedData);
            var result = await converter.Convert(source, destination, amount);
            Assert.True(result.IsSucessfull == true);
        }


        [Theory]
        [InlineData("xxx", "irr", 1000)]
        [InlineData("IRR", "TRX", 1000)] 
        public async Task ShouldNotConvert(string source, string destination, double amount)
        {
            var converter = new CurrencyConverter();
            converter.SetRates(seedData);
            var result = await converter.Convert(source, destination, amount);
            Assert.True(result.IsSucessfull == false);
        }

        [Theory]
        [InlineData("usd", "irr", 1, 42400)]
        [InlineData("IRR", "USD", 42400, 1)]
        [InlineData("EUR", "IRR", 1, 45368)] 
        public async Task ShouldConvertCorrectly(string source, string destination, double amount, double expected)
        {
            var converter = new CurrencyConverter();
            converter.SetRates(seedData);
            var result = await converter.Convert(source, destination, amount);
            Assert.True(result.Result == expected);
        } 
    }
}
