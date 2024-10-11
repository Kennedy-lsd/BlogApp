using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("/api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public StockController(ApplicationDBContext context)
        {
            _context = context;

        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var stocks = await _context.Stocks
            .Select(s => s.ToStockDTO())
            .ToListAsync();

            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var stock = await _context.Stocks.FindAsync(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStockRequestDTO stockDTO)
        {
            var stockForm = stockDTO.ToStockCreateDTO();

            await _context.Stocks.AddAsync(stockForm);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = stockForm.Id }, stockForm.ToStockDTO());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id); 
            // Remainder

            //Find will be faster but for another cases (for exlp. if we need to search by Company Name),
            //but FirstOrDefault is more flexible

            if (stockModel == null)
            {
                return NotFound();
            }

            stockModel.Symbol = updateDTO.Symbol;
            stockModel.CompanyName = updateDTO.CompanyName;
            stockModel.Purchase = updateDTO.Purchase;
            stockModel.LastDiv = updateDTO.LastDiv;
            stockModel.Industry = updateDTO.Industry;
            stockModel.MarketCap = updateDTO.MarketCap;

            await _context.SaveChangesAsync();

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var stockModel = await _context.Stocks.FindAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}