using CloudinaryDotNet.Actions;
using dll;
using dll.Models;
using dll.Products;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Tests
{
    public class ShoppingCartTest
    {
        private readonly ShoppingCart cart;
        private readonly ProductManager productManager;
        public ShoppingCartTest()
        {
            MongoDbManager dbManager = new MongoDbManager(DataManager.ConnectionString(), DataManager.DatabaseName());
            cart = new ShoppingCart(dbManager.Database);
            productManager = new ProductManager(dbManager.Database);
        }

        #region AddItemTest

        [Fact]
        public async Task AddItemTest1Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            Assert.NotNull(cart.Products);
            Assert.Single(cart.Products);
        }

        [Fact]
        public async Task AddItemTest2Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            Assert.NotNull(cart.Products);
            Assert.Equal(2, cart.Products.Count);
        }

        [Fact]
        public async Task AddItemTest3Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);


            Assert.NotNull(cart.Products);
            Assert.Equal(3, cart.Products.Count);
        }

        [Fact]
        public async Task AddItemTest4Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            // dodaje 0 macbookow
            Exception exception = Assert.Throws<Exception>(() =>
                cart.AddItem(product, 0)
            );

            Assert.Equal("Quantity must be greater than zero", exception.Message);

            Assert.NotNull(cart.Products);
            Assert.Equal(3, cart.Products.Count);
        }

        [Fact]
        public async Task AddItemTest5Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            // dodaje -3 macbookow
            Exception exception = Assert.Throws<Exception>(() =>
                cart.AddItem(product, -3)
            );

            Assert.Equal("Quantity must be greater than zero", exception.Message);

            Assert.NotNull(cart.Products);
            Assert.Equal(3, cart.Products.Count);
        }

        [Fact]
        public async Task AddItemTest6Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            

            Exception exception = Assert.Throws<Exception>(() =>
                cart.AddItem(product, 16)
            );


            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            Assert.Equal("Not enough in stock, max: 15", exception.Message);
            Assert.NotNull(cart.Products);
            Assert.Equal(2, cart.Products.Count);
        }


        #endregion 


        #region TotalAmountTest

        [Fact]
        public void TotalAmountTest()
        {
            decimal price = cart.TotalAmount;
            Assert.Equal(0, price);
        }

        [Fact]
        public async Task TotalAmountTest1Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            decimal price = cart.TotalAmount;
            Assert.Equal(500, price);
        }

        [Fact]
        public async Task TotalAmountTest2Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            decimal price = cart.TotalAmount;
            Assert.Equal(3400, price);
        }

        [Fact]
        public async Task TotalAmountTest3Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            decimal price = cart.TotalAmount;
            Assert.Equal(22000, price);
        }

        [Fact]
        public async Task TotalAmountTest4Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            // dodaje 0 macbookow
            Exception exception = Assert.Throws<Exception>(() =>
                cart.AddItem(product, 0)
            );

            Assert.Equal("Quantity must be greater than zero", exception.Message);

            decimal price = cart.TotalAmount;
            Assert.Equal(22000, price);
        }

        [Fact]
        public async Task TotalAmountTest5Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            // dodaje -3 macbookow
            Exception exception = Assert.Throws<Exception>(() =>
                cart.AddItem(product, -3)
            );

            Assert.Equal("Quantity must be greater than zero", exception.Message);

            decimal price = cart.TotalAmount;
            Assert.Equal(22000, price);
        }

        #endregion


        #region ProductsTest

        [Fact]
        public void ProductsTest()
        {
            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.Empty(items);
        }

        [Fact]
        public async Task ProductsTest1Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Single(items);
            KeyValuePair<Product, int> first = items.ElementAt(0);
            Assert.Equal("Test products", first.Key.Name);
            Assert.Equal(1, first.Value);
        }

        [Fact]
        public async Task ProductsTest2Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Equal(2, items.Count);

            KeyValuePair<Product, int> first = items.ElementAt(0);
            Assert.Equal("Test products", first.Key.Name);
            Assert.Equal(1, first.Value);

            KeyValuePair<Product, int> second = items.ElementAt(1);
            Assert.Equal("PlayStation 5", second.Key.Name);
            Assert.Equal(1, second.Value);
        }

        [Fact]
        public async Task ProductsTest3Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);


            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Equal(3, items.Count);

            KeyValuePair<Product, int> first = items.ElementAt(0);
            Assert.Equal("Test products", first.Key.Name);
            Assert.Equal(1, first.Value);

            KeyValuePair<Product, int> second = items.ElementAt(1);
            Assert.Equal("PlayStation 5", second.Key.Name);
            Assert.Equal(1, second.Value);

            KeyValuePair<Product, int> third = items.ElementAt(2);
            Assert.Equal("MacBook Air M2", third.Key.Name);
            Assert.Equal(3, third.Value);
        }

        [Fact]
        public async Task ProductsTest4Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            // dodaje 0 macbookow
            Exception exception = Assert.Throws<Exception>(() =>
                cart.AddItem(product, 0)
            );

            Assert.Equal("Quantity must be greater than zero", exception.Message);


            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Equal(3, items.Count);

            KeyValuePair<Product, int> first = items.ElementAt(0);
            Assert.Equal("Test products", first.Key.Name);
            Assert.Equal(1, first.Value);

            KeyValuePair<Product, int> second = items.ElementAt(1);
            Assert.Equal("PlayStation 5", second.Key.Name);
            Assert.Equal(1, second.Value);

            KeyValuePair<Product, int> third = items.ElementAt(2);
            Assert.Equal("MacBook Air M2", third.Key.Name);
            Assert.Equal(3, third.Value);
        }

        [Fact]
        public async Task ProductsTest5Async()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            // dodaje -3 macbookow
            Exception exception = Assert.Throws<Exception>(() =>
                cart.AddItem(product, -3)
            );

            Assert.Equal("Quantity must be greater than zero", exception.Message);


            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Equal(3, items.Count);

            KeyValuePair<Product, int> first = items.ElementAt(0);
            Assert.Equal("Test products", first.Key.Name);
            Assert.Equal(1, first.Value);

            KeyValuePair<Product, int> second = items.ElementAt(1);
            Assert.Equal("PlayStation 5", second.Key.Name);
            Assert.Equal(1, second.Value);

            KeyValuePair<Product, int> third = items.ElementAt(2);
            Assert.Equal("MacBook Air M2", third.Key.Name);
            Assert.Equal(3, third.Value);
        }

        #endregion


        #region RemoveItemTest

        [Fact]
        public async Task RemoveItemTest1()
        {
            string productId = "000000000000000000011111"; // Test products
            Product productRm = await GetRequiredProductAsync(productId);
            cart.AddItem(productRm, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            cart.RemoveItem(productRm);

            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Equal(2, items.Count);

            KeyValuePair<Product, int> second = items.ElementAt(0);
            Assert.Equal("PlayStation 5", second.Key.Name);
            Assert.Equal(1, second.Value);

            KeyValuePair<Product, int> third = items.ElementAt(1);
            Assert.Equal("MacBook Air M2", third.Key.Name);
            Assert.Equal(3, third.Value);
        }

        [Fact]
        public async Task RemoveItemTest2()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            Product productRm = await GetRequiredProductAsync(productId);
            cart.AddItem(productRm, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            cart.RemoveItem(productRm);

            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Equal(2, items.Count);

            KeyValuePair<Product, int> first = items.ElementAt(0);
            Assert.Equal("Test products", first.Key.Name);
            Assert.Equal(1, first.Value);

            KeyValuePair<Product, int> third = items.ElementAt(1);
            Assert.Equal("MacBook Air M2", third.Key.Name);
            Assert.Equal(3, third.Value);
        }

        [Fact]
        public async Task RemoveItemTest3()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            Product productRm = await GetRequiredProductAsync(productId);
            cart.AddItem(productRm, 3);

            cart.RemoveItem(productRm);

            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Equal(2, items.Count);

            KeyValuePair<Product, int> first = items.ElementAt(0);
            Assert.Equal("Test products", first.Key.Name);
            Assert.Equal(1, first.Value);

            KeyValuePair<Product, int> second = items.ElementAt(1);
            Assert.Equal("PlayStation 5", second.Key.Name);
            Assert.Equal(1, second.Value);
        }

        [Fact]
        public async Task RemoveItemTest4()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            Product productRm = await GetRequiredProductAsync(productId);
            cart.AddItem(productRm, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            cart.RemoveItem(productRm);

            // dodaje -3 macbookow
            Exception exception = Assert.Throws<Exception>(() =>
                cart.AddItem(product, -3)
            );

            Assert.Equal("Quantity must be greater than zero", exception.Message);


            IReadOnlyDictionary<Product, int> items = cart.Products;
            Assert.NotEmpty(items);
            Assert.Equal(2, items.Count);

            KeyValuePair<Product, int> first = items.ElementAt(0);
            Assert.Equal("Test products", first.Key.Name);
            Assert.Equal(1, first.Value);

            KeyValuePair<Product, int> third = items.ElementAt(1);
            Assert.Equal("MacBook Air M2", third.Key.Name);
            Assert.Equal(3, third.Value);
        }

        #endregion


        #region ChangeQuantityTest

        [Fact]
        public async Task ChangeQuantityTest1()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            cart.ChangeQuantity(product, 0);

            Assert.NotNull(cart.Products);
            Assert.Equal(2, cart.Products.Count);

            decimal price = cart.TotalAmount;
            Assert.Equal(3400, price);

        }

        [Fact]
        public async Task ChangeQuantityTest2()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            Product productChange = await GetRequiredProductAsync(productId);
            cart.AddItem(productChange, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            cart.ChangeQuantity(productChange, -2);

            Assert.NotNull(cart.Products);
            Assert.Equal(2, cart.Products.Count);

            decimal price = cart.TotalAmount;
            Assert.Equal(19100, price);
        }

        [Fact]
        public async Task ChangeQuantityTest3()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            cart.ChangeQuantity(product, 5);

            Assert.NotNull(cart.Products);
            Assert.Equal(3, cart.Products.Count);

            IReadOnlyDictionary<Product, int> items = cart.Products;
            KeyValuePair<Product, int> changed = items.ElementAt(2);
            Assert.Equal(5, changed.Value);

            decimal price = cart.TotalAmount;
            Assert.Equal(34400, price);
        }

        [Fact]
        public async Task ChangeQuantityTest4()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            Product productChange = await GetRequiredProductAsync(productId);
            cart.AddItem(productChange, 5);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            cart.ChangeQuantity(productChange, 2);

            Assert.NotNull(cart.Products);
            Assert.Equal(3, cart.Products.Count);

            IReadOnlyDictionary<Product, int> items = cart.Products;
            KeyValuePair<Product, int> changed = items.ElementAt(1);
            Assert.Equal(2, changed.Value);

            decimal price = cart.TotalAmount;
            Assert.Equal(24900, price);
        }

        [Fact]
        public async Task ChangeQuantityTest5()
        {
            string productId = "000000000000000000011111"; // Test products
            Product productChange = await GetRequiredProductAsync(productId);
            cart.AddItem(productChange, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f4a"; // macbook air m2 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 3);

            Exception exception = Assert.Throws<Exception>(() =>
                cart.ChangeQuantity(productChange, 16)
            );

            Assert.Equal("nie ma tyle na stanie, max: 15", exception.Message);


            Assert.NotNull(cart.Products);
            Assert.Equal(3, cart.Products.Count);

            decimal price = cart.TotalAmount;
            Assert.Equal(22000, price);
        }

        #endregion


        #region GetTotalAmountTest

        [Fact]
        public async Task GetTotalAmountTest()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            decimal price = cart.GetTotalAmount();
            Assert.Equal(3400, price);
        }

        #endregion


        #region ClearCartTest

        [Fact]
        public async Task ClearCartTest1()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            cart.ClearCart();

            decimal price = cart.GetTotalAmount();
            Assert.Equal(0, price);
            Assert.Empty(cart.Products);
        }

        [Fact]
        public void ClearCartTest2()
        {
            Exception exception = Assert.Throws<Exception>(() =>
                cart.ClearCart()
            );

            Assert.Equal("Cart is empty", exception.Message);


            decimal price = cart.GetTotalAmount();
            Assert.Equal(0, price);
            Assert.Empty(cart.Products);
        }

        #endregion


        #region SubmitAsyncTest

        [Fact]
        public async Task SubmitAsyncTest1()
        {
            string productId = "000000000000000000011111"; // Test products
            Product product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 1);

            productId = "6931a62442ebb44d99ce5f49"; // playstation5 
            product = await GetRequiredProductAsync(productId);
            cart.AddItem(product, 2);

            DeliveryAddress address = new DeliveryAddress()
            {
                City = "Warsaw",
                Street = "Main Street 1",
                PostalCode = "00-001"
            };
            ObjectId userId = new ObjectId("000000000000000000010000");
            (bool success, string? orderId, Exception? error ) result = await cart.SubmitAsync(productManager, userId, address, "card");

            Assert.True(result.success);
            Assert.NotNull(result.orderId);
            Assert.Null(result.error);
        }

        [Fact]
        public async Task SubmitAsyncTest2()
        {
            DeliveryAddress address = new DeliveryAddress()
            {
                City = "Warsaw",
                Street = "Main Street 1",
                PostalCode = "00-001"
            };
            ObjectId userId = new ObjectId("000000000000000000010000");

            (bool success, string? orderId, Exception? error) result = await cart.SubmitAsync(productManager, userId, address, "card");

            Assert.False(result.success);
            Assert.Null(result.orderId);
            Assert.Equal(new Exception("Empty cart").Message, result.error?.Message);
        }

        #endregion

        
        private async Task<Product> GetRequiredProductAsync(string id)
        {
            Product? product = await productManager.GetProductByIdAsync(id);
            if (product is null)
                throw new Exception($"Product with id '{id}' was not found.");
            return product;
        }
    }
}
