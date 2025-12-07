using dll;
using dll.Models;
using dll.Products;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.InProcDataCollector;
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

        private readonly List<Product> productsList = new List<Product>
        {
            new Product { Name = "Zebra", Category = "Audio", Manufacturer = "Sony", Stock = 10, Price = 2000, CreatedAt = new DateTime(2025, 12, 1) },
            new Product { Name = "Apple", Category = "Smartphone", Manufacturer = "Apple", Stock = 8, Price = 3849, CreatedAt = new DateTime(2025, 11, 15) },
            new Product { Name = "Mango", Category = "Monitor", Manufacturer = "Samsung", Stock = 3, Price = 3850, CreatedAt = new DateTime(2025, 10, 20) }
        };

        #region ProductManager

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
            string productId = "4234234"; // zle id

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
            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\test\kot2.jpg");

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
            string imgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\test\zdj.jpg");

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
            string productId = "42134"; // zle id
            string role = "admin"; // ok

            var exception = await Assert.ThrowsAsync<Exception>(async () =>
            {
                await productManager.DeleteProductAsync(productId, role);
            });

            Assert.Equal("item null", exception.Message);
        }

        [Fact]
        public async Task DeleteProductAsyncTest4()
        {
            string productId = "000000000000000000011111"; // ok
            string role = "admin"; // ok

            await productManager.DeleteProductAsync(productId, role);

            Product? deletedProduct = await productManager.GetProductByIdAsync(productId);
            Assert.Null(deletedProduct);
        }

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

            await productManager.AddPhotoProductAsync(product, "path/to/image.jpg");

            string productId = product.Id.ToString();
            string role = "admin";

            await productManager.DeleteProductAsync(productId, role);

            Product? deletedProduct = await productManager.GetProductByIdAsync(productId);
            Assert.Null(deletedProduct);
        }
        #endregion

        #endregion


        #region ProductSort

        [Fact]
        public void SortProductsTest1()
        {
            List<Product> products = new List<Product>();

            List<Product> sortedProducts = products.SortProducts(ProductSortEnum.NAME, true);

            Assert.NotNull(sortedProducts);
            Assert.Empty(sortedProducts);
        }

        [Fact]
        public void SortProductsTest2()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.NAME, true);

            Assert.NotNull(sortedProducts);
            Assert.Equal("Apple", sortedProducts[0].Name);
            Assert.Equal("Mango", sortedProducts[1].Name);
            Assert.Equal("Zebra", sortedProducts[2].Name);
        }

        [Fact]
        public void SortProductsTest3()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.NAME, false);

            Assert.NotNull(sortedProducts);
            Assert.Equal("Zebra", sortedProducts[0].Name);
            Assert.Equal("Mango", sortedProducts[1].Name);
            Assert.Equal("Apple", sortedProducts[2].Name);
        }

        [Fact]
        public void SortProductsTest4()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.CATEGORY, true);

            Assert.NotNull(sortedProducts);
            Assert.Equal("Audio", sortedProducts[0].Category);
            Assert.Equal("Monitor", sortedProducts[1].Category);
            Assert.Equal("Smartphone", sortedProducts[2].Category);
        }

        [Fact]
        public void SortProductsTest5()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.CATEGORY, false);

            Assert.NotNull(sortedProducts);
            Assert.Equal("Smartphone", sortedProducts[0].Category);
            Assert.Equal("Monitor", sortedProducts[1].Category);
            Assert.Equal("Audio", sortedProducts[2].Category);
        }

        [Fact]
        public void SortProductsTest6()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.STOCK, true);

            Assert.NotNull(sortedProducts);
            Assert.Equal(3, sortedProducts[0].Stock);
            Assert.Equal(8, sortedProducts[1].Stock);
            Assert.Equal(10, sortedProducts[2].Stock);
        }

        [Fact]
        public void SortProductsTest7()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.STOCK, false);

            Assert.NotNull(sortedProducts);
            Assert.Equal(10, sortedProducts[0].Stock);
            Assert.Equal(8, sortedProducts[1].Stock);
            Assert.Equal(3, sortedProducts[2].Stock);
        }

        [Fact]
        public void SortProductsTest8()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.PRICE, true);

            Assert.NotNull(sortedProducts);
            Assert.Equal(2000, sortedProducts[0].Price);
            Assert.Equal(3849, sortedProducts[1].Price);
            Assert.Equal(3850, sortedProducts[2].Price);
        }
        
        [Fact]
        public void SortProductsTest9()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.PRICE, false);

            Assert.NotNull(sortedProducts);
            Assert.Equal(3850, sortedProducts[0].Price);
            Assert.Equal(3849, sortedProducts[1].Price);
            Assert.Equal(2000, sortedProducts[2].Price);
        }
        
        [Fact]
        public void SortProductsTest10()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.CREATED_AT, true);
            // od najstarszego
            Assert.NotNull(sortedProducts); 
            Assert.Equal(new DateTime(2025, 10, 20), sortedProducts[0].CreatedAt);
            Assert.Equal(new DateTime(2025, 11, 15), sortedProducts[1].CreatedAt);
            Assert.Equal(new DateTime(2025, 12, 1), sortedProducts[2].CreatedAt);
        }

        [Fact]
        public void SortProductsTest11()
        {
            List<Product> sortedProducts = productsList.SortProducts(ProductSortEnum.CREATED_AT, false);
            // od najbardzeij ostatniego
            Assert.NotNull(sortedProducts);
            Assert.Equal(new DateTime(2025, 12, 1), sortedProducts[2].CreatedAt);
            Assert.Equal(new DateTime(2025, 11, 15), sortedProducts[1].CreatedAt);
            Assert.Equal(new DateTime(2025, 10, 20), sortedProducts[0].CreatedAt);
        }

        #endregion


        #region ProductFilter

        [Fact]
        public void FilterProductsTest1()
        {
            List<Product> sortedProducts = productsList.FilterByCategory("Monitor");

            Assert.NotNull(sortedProducts);
            Assert.Equal("Mango", sortedProducts[0].Name);
            Assert.Equal("Monitor", sortedProducts[0].Category);
        }

        [Fact]
        public void FilterProductsTest2()
        {
            List<Product> sortedProducts = productsList.FilterByManufacturer("Sony");

            Assert.NotNull(sortedProducts);
            Assert.Equal("Zebra", sortedProducts[0].Name);
            Assert.Equal("Audio", sortedProducts[0].Category);
        }

        [Fact]
        public void FilterProductsTest3()
        {
            List<Product> sortedProducts = productsList.FilterByPriceRange(3500,4000);

            Assert.NotNull(sortedProducts);
            Assert.Equal("Apple", sortedProducts[0].Name);
            Assert.Equal("Smartphone", sortedProducts[0].Category);
            Assert.Equal("Mango", sortedProducts[1].Name);
            Assert.Equal("Monitor", sortedProducts[1].Category);
        }

        [Fact]
        public void FilterProductsTest4()
        {
            List<Product> sortedProducts = productsList.FilterByStock(10);

            Assert.NotNull(sortedProducts);
            Assert.Equal("Zebra", sortedProducts[0].Name);
            Assert.Equal("Audio", sortedProducts[0].Category);
        }

        [Fact]
        public void FilterProductsTest5()
        {
            List<Product> sortedProducts = productsList.SearchByName("smartphone");

            Assert.NotNull(sortedProducts);
            Assert.Equal("Apple", sortedProducts[0].Name);
            Assert.Equal("Smartphone", sortedProducts[0].Category);
        }

        [Fact]
        public void FilterProductsTest6()
        {
            List<Product> sortedProducts = productsList.FilterByPriceRange(3500, 4000).FilterByManufacturer("Samsung");

            Assert.NotNull(sortedProducts);
            Assert.Equal("Mango", sortedProducts[0].Name);
            Assert.Equal("Monitor", sortedProducts[0].Category);
        }



        #endregion
    }
}
