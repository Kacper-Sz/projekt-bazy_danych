using dll.Models;
using dll.Orders;
using dll.Products;
using dll.Users;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll
{
    public sealed class ShoppingCart
    {
        private const string COLLECTION_NAME = "orders";
        private readonly IMongoCollection<Order> _orders;

        private Dictionary<Product, int> products = new Dictionary<Product, int>();

        private decimal totalAmount = 0;


        public ShoppingCart()
        {
            MongoDbManager mongoDbManager = new MongoDbManager();
            _orders = mongoDbManager.Database.GetCollection<Order>(COLLECTION_NAME);

        }


        public decimal TotalAmount => totalAmount;
        public IReadOnlyDictionary<Product, int> Products => products;

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new Exception("Quantity must be greater than zero");

            if (quantity > product.Stock)
                throw new Exception($"Not enough in stock, max: {product.Stock}");

            products[product] = products.TryGetValue(product, out int existingQuantity)
                ? existingQuantity + quantity
                : quantity;

            totalAmount = products.Sum(p => p.Key.Price * p.Value);
        }

        // zakladamy ze jak produkt sie wyswietla w koszyku to w nim istnieje
        public void RemoveItem(Product product)
        {
            totalAmount -= products[product] * product.Price;
            products.Remove(product);
        }

        public void ChangeQuantity(Product product,int newQuantity)
        {
            if (newQuantity <= 0)
            {
                RemoveItem(product);
                return;
            }

            if (newQuantity > product.Stock)
                throw new Exception($"nie ma tyle na stanie, max: {product.Stock}");

            totalAmount -= products[product] * product.Price;
            products[product] = newQuantity;
            totalAmount += product.Price * newQuantity;
        }

        public decimal GetTotalAmount() =>
            products.Sum(p => p.Key.Price * p.Value);

        public void ClearCart()
        {
            if (products.Count != 0)
            {
                products.Clear();
                totalAmount = 0;
            }
            else
            {
                throw new Exception("Cart is empty");
            }
        }


        public async Task<(bool success, string? orderId, Exception? error)> SubmitAsync(ProductManager productManager,
            ObjectId customerId,
            DeliveryAddress address,
            string paymentMethod)
        {

            if (Products.Count == 0)
                return (false, null, new Exception("Empty cart"));

            Order order = new Order
            {
                CustomerId = customerId,
                Items = Products.Select(prod => new OrderItem()
                {
                    ProductId = prod.Key.Id,
                    Quantity = prod.Value,
                }).ToList(),
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = TotalAmount,
                DeliveryAddress = address,
                PaymentMethod = paymentMethod
            };

            await _orders.InsertOneAsync(order);

            ClearCart();

            return (true, order.Id.ToString(), null);
        }

    }
}
