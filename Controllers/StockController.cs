using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("/api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _stockRepo;
        public StockController(IStockRepository stockRepo)
        {
            _stockRepo = stockRepo;

        }
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] QueryObject query)
        {
            var isValid = await _stockRepo.IsStockValid(ModelState);

            if (isValid != null)
            {
                return BadRequest(isValid);
            }

            var stocks = await _stockRepo.GetAllStocksAsync(query);

            var stockDTO = stocks.Select(x => x.ToStockDTO());

            return Ok(stockDTO);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepo.GetStockByIdAsync(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateStockRequestDTO stockDTO)
        {
            var isValid = await _stockRepo.IsStockValid(ModelState);

            if (isValid != null)
            {
                return BadRequest(isValid);
            }

            var stockForm = stockDTO.ToStockCreateDTO();

            var createdStock = await _stockRepo.CreateStockAsync(stockForm);

            return CreatedAtAction(nameof(GetById), new { id = createdStock.Id }, createdStock.ToStockDTO());
        }


        [HttpPut("{id:int}")]
        public async Task<ActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            try
            {
                var isValid = await _stockRepo.IsStockValid(ModelState);

                if (isValid != null)
                {
                    return BadRequest(isValid);
                }

                var updatedStock = await _stockRepo.UpdateStockAsync(id, updateDTO);

                if (updatedStock == null)
                {
                    return NotFound();
                }

                return Ok(updatedStock.ToStockDTO());
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle optimistic concurrency issues
                return Conflict(new { message = "The stock entry was modified or deleted by another process." });
            }
        }


        [HttpDelete("{id:int}")]

        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            var isValid = await _stockRepo.IsStockValid(ModelState);

            if (isValid != null)
            {
                return BadRequest(isValid);
            }

            var stockModel = await _stockRepo.DeleteStockAsync(id);
            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}