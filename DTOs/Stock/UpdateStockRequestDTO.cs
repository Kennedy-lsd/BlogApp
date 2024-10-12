using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Stock
{
    public class UpdateStockRequestDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Symbol must be at least 3 charachters")]
        [MaxLength(100)]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        [MinLength(3, ErrorMessage = "Company Name must be at least 3 charachters")]
        [MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000)]
        public decimal Purchase { get; set; }

        [Required]
        [Range(0.001, 100)]
        public decimal LastDiv { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Industry can't be over 20 characters")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Range(0, 10000000)]
        public long MarketCap { get; set; }
    }
}