using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities; 
using ConversionPath.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConversionPath.Persistence.ExchangeRateAggregate.Repositories
{
    public class ExchangeRateRepository : RepositoryBase<ExchangeRate>
    {
        public ExchangeRateRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<ValidationResult<ExchangeRate>> Update(int id, ExchangeRate exchangeRate)
        {
            var result = await _domainCollection.Update(id, exchangeRate);
            if (result.IsSuccessfull)
            {
                var exchangeRateToUpdate = await _dbContext.ExchangeRates.FirstAsync(r => r.Id == id);
                exchangeRateToUpdate.SourceCurrency = exchangeRate.SourceCurrency;
                exchangeRateToUpdate.DestinationCurrency = exchangeRate.DestinationCurrency;
                exchangeRateToUpdate.Rate = exchangeRate.Rate;
                exchangeRateToUpdate.DateTime = exchangeRate.DateTime;

                _dbContext.Entry(exchangeRateToUpdate).State =EntityState.Modified; 
            }
            return result;
        }
    }
}
