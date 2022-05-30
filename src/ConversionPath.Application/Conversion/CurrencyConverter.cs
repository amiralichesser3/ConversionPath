using ConversionPath.Application.ExchangeRates.Queries;
using ConversionPath.Domain.Contracts;
using ConversionPath.Domain.ExchangeRates.Entities;
using ConversionPath.Shared.Dtos;
using ConversionPath.Shared.Dtos.ExchangeRates;
using MediatR;
using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;

namespace ConversionPath.Application.Conversion;

public class CurrencyConverter: ICurrencyConverter
{ 
    private ICollection<ExchangeRateDto> allRates;
    private Graph<int, string> graph = new Graph<int, string>();
    public CurrencyConverter()
    {
        allRates = new List<ExchangeRateDto>(); 
    }

    public async Task<ConversionResultDto> Convert(string sourceCurrency, string destinationCurrency, double amount)
    { 
        var result = new ConversionResultDto();
        if (sourceCurrency == destinationCurrency)
        {
            result.IsSucessfull = true;
            result.Result = amount;
            result.RatesUsed.Add(new ExchangeRateDto { Id = 0, SourceCurrency = sourceCurrency, DestinationCurrency = destinationCurrency, Rate = 1, DateTime = DateTime.Now });
            return result;
        } 

        var source1 = allRates.FirstOrDefault(r => r.SourceCurrency.ToLower() == sourceCurrency.ToLower())?.SourceNodeId;
        var source2 = allRates.FirstOrDefault(r => r.DestinationCurrency.ToLower() == sourceCurrency.ToLower())?.DestinationNodeId;
        var source = (uint?)(source1 ?? source2);

        var destination1 = allRates.FirstOrDefault(r => r.SourceCurrency.ToLower() == destinationCurrency.ToLower())?.SourceNodeId;
        var destination2 = allRates.FirstOrDefault(r => r.DestinationCurrency.ToLower() == destinationCurrency.ToLower())?.DestinationNodeId;
        var destination = (uint?)(destination1 ?? destination2);

        if (source == null || destination == null)
        {
            return result;
        }

        var path = graph.Dijkstra((uint)source, (uint)destination).GetPath().ToList();

        if (!path.Any()) return result;
        result.Result = amount;
        result.IsSucessfull = true;
        for (int i = 0; i < path.Count() - 1; i++)
        {
            var rate = allRates.FirstOrDefault(r => r.SourceNodeId == path[i] && r.DestinationNodeId == path[i + 1]);
            if (rate != null)
            {
                result.RatesUsed.Add(rate);
                result.Result = rate.Rate * result.Result;
            }
            else
            {
                rate = allRates.FirstOrDefault(r => r.SourceNodeId == path[i+1] && r.DestinationNodeId == path[i]);
                if (rate != null)
                {
                    result.RatesUsed.Add(rate);
                    result.Result = result.Result / rate.Rate;
                }
            }
        }
        return result;
    }

    public void ResetRates()
    {
        allRates.Clear();
        graph = new Graph<int, string>();
    }

    public void SetRates(ICollection<ExchangeRateDto> rates)
    {
        graph = new Graph<int, string>();
        allRates = rates;
        LoadGraph();
    }

    private void LoadGraph()
    {
        var distinctCurrencies = allRates.Select(r => r.SourceCurrency).Union(allRates.Select(r => r.DestinationCurrency)).Distinct();

        uint i = 0;

        foreach (var currency in distinctCurrencies)
        {
            i++;
            graph.AddNode((int)i);
            var s = allRates.Where(r => r.SourceCurrency == currency);
            foreach (var item in s)
            {
                item.SourceNodeId = i;
            }
            var d = allRates.Where(r => r.DestinationCurrency == currency);
            foreach (var item in d)
            {
                item.DestinationNodeId = i;
            }
        }

        foreach (var rate in allRates)
        {
            graph.Connect(rate.SourceNodeId, rate.DestinationNodeId, 1, string.Empty);
            graph.Connect(rate.DestinationNodeId, rate.SourceNodeId, 1, string.Empty);
        }
    } 
}