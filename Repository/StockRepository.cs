using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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


        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks.Include(x => x.Comments).ToListAsync();
        }

        public async Task<Stock?> GetStockByIdAsync(int id)
        {
            return await _context.Stocks.Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDTO updateDTO)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

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
    }
}