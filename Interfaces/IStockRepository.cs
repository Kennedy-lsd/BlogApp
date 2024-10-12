using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

        /// <summary>
        /// Validates the comment by checking the provided ModelStateDictionary.
        /// </summary>
        /// <param name="modelState">The ModelStateDictionary containing validation information.</param>
        /// <returns>
        /// Returns the ModelStateDictionary if it is invalid, otherwise returns null.
        /// </returns>
        Task<ModelStateDictionary?> IsStockValid(ModelStateDictionary ModelState);
    }
}