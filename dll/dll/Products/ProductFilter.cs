using dll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace dll.Products
{
    public static class ProductFilter
    {
        public static List<Product> FilterByCategory(this List<Product> products, string category) =>
            products.Where(p => p.Category == category).ToList();

        public static List<Product> FilterByManufacturer(this List<Product> products, string manufacturer) =>
            products.Where(p => p.Manufacturer == manufacturer).ToList();

        public static List<Product> FilterByPriceRange(this List<Product> products, decimal minPrice, decimal maxPrice) =>
            products.Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();

        public static List<Product> FilterByStock(this List<Product> products, int minStock) =>
            products.Where(p => p.Stock >= minStock).ToList();

        public static List<Product> SearchByName(this List<Product> products, string searchTerm) =>
            products.Where(p =>
                (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.Description) && p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.Category) && p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (!string.IsNullOrEmpty(p.Manufacturer) && p.Manufacturer.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                (p.Specs != null && p.Specs.Values.Any(v => v != null && v.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
            ).ToList();
    }
}
