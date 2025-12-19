using App.ViewModels;
using dll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Wrappers
{
    class CartItem : BaseViewModel
    {
        public Product Product { get; }

        private int quantity;
        public int Quantity
        {
            get => quantity;
            set => SetProperty(ref quantity, value);
        }

        public string Name => Product.Name; 
        public string ImageUrl => Product.ImageUrl;
        public decimal Price => Product.Price;
        public decimal TotalPrice => Product.Price * Quantity;

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }
    }
}
