using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dll.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace dll.Products
{
    public class ProductManager
    {

        private const string COLLECTION_NAME = "products";

        private readonly IMongoCollection<Product> _products;

        private readonly Cloudinary _cloudinary;
        private readonly Account _account;

        public ProductManager()
        {
            MongoDbManager mongoDbManager = new MongoDbManager();
            _products = mongoDbManager.Database.GetCollection<Product>(COLLECTION_NAME);

            _account = new Account(
                "dv1nk0kbi",
                "989932878854628",
                "U0TsweDyygH_mYnfGelE-I_MIzI");

            _cloudinary = new Cloudinary(_account);
            _cloudinary.Api.Secure = true;
        }

        public async Task AddPhotoProductAsync(Product product, string imagePath)
        {
            ImageUploadParams uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imagePath),
            };

            ImageUploadResult uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exception("Error occurred while uploading photo");

            product.ImageUrl = uploadResult.SecureUrl.ToString();
            product.CreatedAt = DateTime.Now;

            if (!ValidateProductData(product))
                throw new Exception("Invalid product data");

            await _products.InsertOneAsync(product);
        }

        private bool ValidateProductData(Product product)
            => product != null
            && !string.IsNullOrEmpty(product.Name)
            && !string.IsNullOrEmpty(product.Manufacturer)
            && !string.IsNullOrEmpty(product.Category)
            && product.Price > 0
            && product.Price >= 0
            && !string.IsNullOrEmpty(product.ImageUrl)
            && product.Specs != null && product.Specs.All(spec => !string.IsNullOrWhiteSpace(spec.Key) && !string.IsNullOrWhiteSpace(spec.Value))
            && !string.IsNullOrEmpty(product.Description)
            && product.CreatedAt != default;

        public async Task DeleteProductAsync(string id, string userRole)
        {
            if (userRole != "admin")
                throw new Exception("not admin");

            Product? item = await GetProductByIdAsync(id);
            if (item == null)
                throw new Exception("item null");

            string publicId = Path.GetFileNameWithoutExtension(item.ImageUrl);

            DeletionParams deletionParams = new DeletionParams(publicId);
            DeletionResult deletionResult = await _cloudinary.DestroyAsync(deletionParams);

            if (deletionResult.Result != "ok")
            {
                throw new Exception("Deletion failed");
            }

            await _products.DeleteOneAsync(p => p.Id == new ObjectId(id));

        }

        public async Task<Product?> GetProductByIdAsync(string id)
        {
            ObjectId productId = new ObjectId(id);
            return await _products.Find(p => p.Id == productId).FirstOrDefaultAsync();
        }

        public async Task<List<Product>> GetAllProductsAsync() =>
             await _products.Find(_=>true).ToListAsync();

        public async Task<List<string>> GetDistinctManufacturersAsync()
        {
            FilterDefinition<Product> filter = Builders<Product>.Filter.Gte(p => p.Stock, 1);
            return await _products.Distinct<string>("manufacturer", filter).ToListAsync();
        }

        public async Task<ProductUpdateStockResult> UpdateStockAsync(string id, int newStock, string userRole)
        {
            try
            {
                if (userRole != "admin")
                    return ProductUpdateStockResult.UNAUTHORIZED;

                ObjectId productId = new ObjectId(id);

                if (newStock <= 0)
                    return ProductUpdateStockResult.ERROR;

                UpdateDefinition<Product> update = Builders<Product>.Update.Set(p => p.Stock, newStock);
                UpdateResult result = await _products.UpdateOneAsync(p => p.Id == productId, update);

                if (result.MatchedCount == 0)
                    return ProductUpdateStockResult.NOT_FOUND;

                return ProductUpdateStockResult.SUCCESS;
            }
            catch (Exception)
            {
                return ProductUpdateStockResult.ERROR;
            }
        }
    }
}
