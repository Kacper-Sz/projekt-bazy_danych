using dll.Models;
using dll.Orders;
using dll.Products;
using dll.Users;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll
{
    public sealed class ShoppingCart
    {
        public ShoppingCart() { }

        // private List<Product> products = new List<Product>();

        private Dictionary<Product, int> products = new Dictionary<Product, int>();

        private decimal totalAmount = 0;



        public decimal TotalAmount => totalAmount;
        public IReadOnlyDictionary<Product, int> Products => products;

        // tez nie wiem w ktorym miejscu najlepiej sprawdzic czy w ogole mozna dodac to do koszyka
        // (czy jest taka ilosc w magazynie)
        // na razie dam tutaj
        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
            {
                throw new Exception("Quantity must be grater than zero");
            }

            if (quantity <= product.Stock)
            {

                if (products.ContainsKey(product))
                {
                    products[product] += quantity;
                }
                else
                {
                    products.Add(product, quantity);
                }

            }

            totalAmount = products.Sum(p => p.Key.Price * p.Value);
        }

        // troche nie wiem jak obecnie ma wygladac usuwanie 
        // bo dane beda brane z elementu na ktorego klikniemy czy cos

        // zakladam ze jak produkt sie wyswietla w koszyku to w nim istnieje
        // dlatego nie sprawdzam czy jest
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
                throw new Exception("error");

            totalAmount -= products[product] * product.Price;
            products[product] = newQuantity;
            totalAmount += product.Price * newQuantity;
        }

        public decimal GetTotalAmount() =>
            products.Sum(p => p.Key.Price * p.Value);

        public void ClearCart()
        {
            products.Clear();
            totalAmount = 0;
        }


        // trzeba sprawdzic czy jest zalogowany
        // tu jeszcze zmienic bledy na np enum
        public async Task<(bool success, string? orderId, string? error)> SubmitAsync(IMongoDatabase database,
            ProductManager productManager,
            ObjectId customerId,
            DeliveryAddress address,
            string paymentMethod)
        {
            if (Products.Count == 0)
                return (false, null, "koszyk pusty");

            foreach (var prod in Products)
            {
                Product product = prod.Key;
                int prodQuantity = prod.Value;

                
                if (prodQuantity <= 0)
                    return (false, null, "za malo");

                if (prodQuantity > product.Stock)
                    return (false, null, "za dzuo");
            }

            foreach (var prod in Products)
            {
                Product product = prod.Key;
                int prodQuantity = prod.Value;

                var ok = await productManager.UpdateAsync(product.Id.ToString(), prodQuantity);
                if (!ok)
                    return (false, null, "nie ma tyle na stanie");
            }

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

            IMongoCollection<Order> orders = database.GetCollection<Order>("orders");
            await orders.InsertOneAsync(order);

            ClearCart();

            return (true, order.Id.ToString(), null);
        }

    }
}
