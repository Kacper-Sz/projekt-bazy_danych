using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.Models
{
    public class ProductFilterOptions
    {
        public string? Category { get; set; }
        public string? Manufacturer { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinStock { get; set; }
        public string? SearchTerm { get; set; }
    }

}
