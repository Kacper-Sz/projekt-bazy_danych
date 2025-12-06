using dll;
using dll.Models;
using dll.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ProductManagerTest
    {
        private readonly ProductManager productManager;
        public ProductManagerTest()
        {
            MongoDbManager dbManager = new MongoDbManager(DataManager.ConnectionString(), DataManager.DatabaseName());
            productManager = new ProductManager(dbManager.Database);
        }

        #region GetProductByIdAsync

        [Fact]
        public async Task GetProductByIdAsyncTest1()
        {
            string productId = "6931a62442ebb44d99ce5f49";

            Product? product = await productManager.GetProductByIdAsync(productId);

            Assert.NotNull(product);
            Assert.Equal("PlayStation 5", product.Name);
        }

        [Fact]
        public async Task GetProductByIdAsyncTest2()
        {
            string productId = "6931a62442ebb44d99ce5f47";

            Product? product = await productManager.GetProductByIdAsync(productId);

            Assert.NotNull(product);
            Assert.Equal("Dell XPS 13 Laptop", product.Name);
        }

        [Fact]
        public async Task GetProductByIdAsyncTest3()
        {
            string productId = "6931a62442ebb44d99ce5f50";

            Product? product = await productManager.GetProductByIdAsync(productId);

            Assert.NotNull(product);
            Assert.Equal("Tablets", product.Category);
        }

        [Fact]
        public async Task GetProductByIdAsyncTest4()
        {
            string productId = "6931ad234000044d99ce5f50"; // zle id

            Product? product = await productManager.GetProductByIdAsync(productId);

            Assert.Null(product);
        }

        #endregion

        #region GetAllProductsAsync

        [Fact]
        public async Task GetAllProductsAsyncTest()
        {
            List<Product> products=  await productManager.GetAllProductsAsync();
            Assert.NotNull(products);
            Assert.True(products.Count >= 50);
            Assert.Contains(products, p => p.Name == "iPad Pro 12.9");
            Assert.Contains(products, p => p.Category == "Laptops");
            Assert.DoesNotContain(products, p => p.Name == "Towar");
        }
        #endregion

        #region GetDistinctManufacturersAsync

        [Fact]
        public async Task GetDistinctManufacturersAsyncTest()
        {
            List<string> manufacturers = await productManager.GetDistinctManufacturersAsync();
            Assert.NotNull(manufacturers);
            Assert.Contains("Apple", manufacturers);
            Assert.Contains("Dell", manufacturers);
            Assert.DoesNotContain("Towar", manufacturers);
        }

        #endregion

        #region UpdateStockAsync

        [Fact]
        public async Task UpdateStockAsyncTest1()
        {
            string productId = "000000000000000000000001";
            int newStock = 10;
            string? userRole = "user";

            ProductUpdateStockResult result = await productManager.UpdateStockAsync(productId, newStock, userRole);

            Assert.Equal(ProductUpdateStockResult.UNAUTHORIZED, result);
        }

        [Fact]
        public async Task UpdateStockAsyncTest2()
        {
            string productId = "000000120040500000000001";
            int newStock = 20;
            string? userRole = "admin";

            ProductUpdateStockResult result = await productManager.UpdateStockAsync(productId, newStock, userRole);

            Assert.Equal(ProductUpdateStockResult.NOT_FOUND, result);
        }

        [Fact]
        public async Task UpdateStockAsyncTest3()
        {
            string productId = "000000120040500000000001";
            int newStock = -5;
            string? userRole = "admin";

            ProductUpdateStockResult result = await productManager.UpdateStockAsync(productId, newStock, userRole);

            Assert.Equal(ProductUpdateStockResult.ERROR, result);
        }

        [Fact]
        public async Task UpdateStockAsyncTest4()
        {
            string productId = "000000000000000000000001";
            int newStock = 25;
            string? userRole = "admin";

            Product? product = await productManager.GetProductByIdAsync(productId);
            int stockBefore = product.Stock;

            ProductUpdateStockResult result = await productManager.UpdateStockAsync(productId, newStock, userRole);

            product = await productManager.GetProductByIdAsync(productId);
            int stockAfter = product.Stock;

            Assert.NotNull(product);
            Assert.Equal(ProductUpdateStockResult.SUCCESS, result);
            Assert.Equal(newStock, stockAfter);
            Assert.NotEqual(stockBefore, stockAfter);
        }

        #endregion

        #region AddPhotoProductAsync
        [Fact]
        public async Task AddPhotoProductAsyncTest1()
        {
            Product product = new Product()
            {
                Name = "Test Product",
                Manufacturer = "Test Manufacturer",
                Category = "Test Category",
                Price = 100,
                Specs = new Dictionary<string, string> { { "Spec1", "Value1" } },
                Description = "Test Description"
            };
            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\test\kot2.jpg");

            await productManager.AddPhotoProductAsync(product, imgPath);

            Product? addedProduct = await productManager.GetProductByIdAsync(product.Id.ToString());
            Assert.NotNull(addedProduct);
            Assert.Equal(product.ImageUrl, addedProduct.ImageUrl);
            Assert.Equal(product.Name, addedProduct.Name);
        }

        [Fact]
        public async Task AddPhotoProductAsyncTest2()
        {
            Product product = new Product()
            {
                Name = "", // zla nazwa
                Manufacturer = "Test Manufacturer",
                Category = "Test Category",
                Price = -100, // zla cena
                Specs = new Dictionary<string, string> { { "Spec1", "Value1" } },
                Description = "Test Description"
            };
            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\test\zdj.jpg");

            var exception = await Assert.ThrowsAsync<Exception>(async () => 
                await productManager.AddPhotoProductAsync(product, imgPath)
            );

            Assert.Equal("Invalid product data", exception.Message);
        }
        #endregion

        #region DeleteProductAsync

        [Fact]
        public async Task DeleteProductAsyncTest1()
        {
            string productId = "000000000000000000011111"; // ok
            string role = "customer"; // nie admin

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await productManager.DeleteProductAsync(productId, role);
            });

            Assert.Equal("not admin", exception.Message);
        }

        [Fact]
        public async Task DeleteProductAsyncTest2()
        {
            string productId = "9238891"; // zle id
            string role = "user"; // nie admin

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await productManager.DeleteProductAsync(productId, role);
            });

            Assert.Equal("not admin", exception.Message);
        }

        [Fact]
        public async Task DeleteProductAsyncTest3()
        {
            string productId = "000030400100000000011111"; // zle id
            string role = "admin"; // ok

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await productManager.DeleteProductAsync(productId, role);
            });

            Assert.Equal("item null", exception.Message);
        }

        /*
        // brakuje linku w bazie danych
        [Fact]
        public async Task DeleteProductAsyncTest4()
        {
            string productId = "000000000000000000011111"; // ok
            string role = "admin"; // ok

            await productManager.DeleteProductAsync(productId, role);

            Product? deletedProduct = await productManager.GetProductByIdAsync(productId);
            Assert.Null(deletedProduct);
        }
        */
        [Fact]
        public async Task DeleteProductAsyncTest5()
        {
            Product product = new Product
            {
                Name = "Test Product",
                Manufacturer = "Test Manufacturer",
                Category = "Test Category",
                Price = 100,
                Specs = new Dictionary<string, string> { { "Spec1", "Value1" } },
                Description = "Test Description",
                CreatedAt = DateTime.Now
            };

            await productManager.AddPhotoProductAsync(product, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\test\kot2.jpg"));

            string productId = product.Id.ToString();
            string role = "admin";

            await productManager.DeleteProductAsync(productId, role);

            Product? deletedProduct = await productManager.GetProductByIdAsync(productId);
            Assert.Null(deletedProduct);
        }
        #endregion

        
    }
}
