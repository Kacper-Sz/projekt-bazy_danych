using dll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.Products
{
    public static class ProductSort
    {
        public static List<Product> SortProducts(this List<Product> products, ProductSortEnum sortBy, bool ascending = true)
        {
            if (products == null || products.Count == 0)
                return products;

            var sorted = sortBy switch
            {
                ProductSortEnum.NAME => ascending
                    ? products.OrderBy(p => p.Name)
                    : products.OrderByDescending(p => p.Name),

                ProductSortEnum.CATEGORY => ascending
                    ? products.OrderBy(p => p.Category)
                    : products.OrderByDescending(p => p.Category),

                ProductSortEnum.STOCK => ascending
                    ? products.OrderBy(p => p.Stock)
                    : products.OrderByDescending(p => p.Stock),

                ProductSortEnum.PRICE => ascending
                    ? products.OrderBy(p => p.Price)
                    : products.OrderByDescending(p => p.Price),

                ProductSortEnum.CREATED_AT => ascending
                    ? products.OrderBy(p => p.CreatedAt)
                    : products.OrderByDescending(p => p.CreatedAt),

                _ => products.AsEnumerable()
            };

            return sorted.ToList();
        }
    }
}
