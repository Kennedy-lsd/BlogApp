using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateStockAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> DeleteStockAsync(int id)
        {
            var stockModel = await _context.Stocks.FindAsync(id);

            if (stockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }


        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(x => x.Comments).ThenInclude(c => c.AppUser).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(x => x.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(x => x.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDecsending
                    ? stocks.OrderByDescending(x => x.Symbol)
                    : stocks.OrderBy(x => x.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.Include(x => x.Comments).ThenInclude(c => c.AppUser).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDTO updateDTO)
        {
            var stock = await _context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);

            if (stock == null)
            {
                return null;
            }

            stock.Symbol = updateDTO.Symbol;
            stock.CompanyName = updateDTO.CompanyName;
            stock.Purchase = updateDTO.Purchase;
            stock.LastDiv = updateDTO.LastDiv;
            stock.Industry = updateDTO.Industry;
            stock.MarketCap = updateDTO.MarketCap;

            await _context.SaveChangesAsync();

            return stock;
        }

        public Task<bool> StockExistAsync(int stockId)
        {
            return _context.Stocks.AnyAsync(x => x.Id == stockId);
        }

        public Task<ModelStateDictionary?> IsStockValid(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                return Task.FromResult<ModelStateDictionary?>(modelState);
            }

            return Task.FromResult<ModelStateDictionary?>(null);
        }
    }
}