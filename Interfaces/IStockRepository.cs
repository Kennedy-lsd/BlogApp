using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocksAsync();
        Task<Stock?> GetStockByIdAsync(int id);
        Task<Stock> CreateStockAsync(Stock stockModel);
        Task<bool> StockExistAsync(int stockId);
        Task<Stock?> UpdateStockAsync(int id, UpdateStockRequestDTO updateDTO);
        Task<Stock?> DeleteStockAsync(int id);
    }
}