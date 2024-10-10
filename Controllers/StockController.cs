using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult GetAll()
        {
            var stocks = _context.Stocks
            .Select(s => s.ToStockDTO())
            .ToList();

            return Ok(stocks);
        }
        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute] int id)
        {
            var stock = _context.Stocks.Find(id);

            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock.ToStockDTO());
        }
        [HttpPost]
        public ActionResult Create([FromBody] CreateStockRequestDTO stockDTO)
        {
            var stockForm = stockDTO.ToStockCreateDTO();

            _context.Stocks.Add(stockForm);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), new {id = stockForm.Id}, stockForm.ToStockDTO());
        }

    }
}