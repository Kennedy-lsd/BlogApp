using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;

namespace api.Mappers
{
    public static class StockMapper
    {
        public static StockDTO ToStockDTO(this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                CompanyName = stockModel.CompanyName,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
            };
        }

        public static Stock ToStockCreateDTO(this CreateStockRequestDTO form)
        {
            return new Stock
            {
                Symbol = form.Symbol,
                CompanyName = form.CompanyName,
                Purchase = form.Purchase,
                LastDiv = form.LastDiv,
                Industry = form.Industry,
                MarketCap = form.MarketCap,
            };
        }
    }
}