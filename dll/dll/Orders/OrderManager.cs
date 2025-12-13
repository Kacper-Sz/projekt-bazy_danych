
using dll.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.Orders
{
    public class OrderManager
    {
        private const string COLLECTION_NAME = "orders";
        
        private readonly IMongoCollection<Order> _orders;

        public OrderManager()
        {
            MongoDbManager mongoDbManager = new MongoDbManager();
            _orders = mongoDbManager.Database.GetCollection<Order>(COLLECTION_NAME);
        }

        public async Task<OrderCreationEnum> CreateOrderAsync(Order order)
        {
            if (!ValidateOrderData(order))
                return OrderCreationEnum.INVALID_DATA;
            await _orders.InsertOneAsync(order);
            return OrderCreationEnum.SUCCESS;
        }

        private bool ValidateOrderData(Order order)
            => order.CustomerId != ObjectId.Empty
            && order.Items != null && order.Items.Count > 0
            && !string.IsNullOrEmpty(order.Status)
            && order.CreatedAt != default
            && order.DeliveryAddress != null && !string.IsNullOrEmpty(order.DeliveryAddress.City) && !string.IsNullOrEmpty(order.DeliveryAddress.Street) && !string.IsNullOrEmpty(order.DeliveryAddress.PostalCode)
            && !string.IsNullOrEmpty(order.PaymentMethod);

        public async Task<Order?> GetOrderByIdAsync(string id)
        {
            ObjectId objectId = new ObjectId(id);
            return await _orders.Find(o => o.Id == objectId).FirstOrDefaultAsync();
        }

        public async Task<List<Order>> GetOrdersByCustomerAsync(string customerId) 
        {
            ObjectId objectId = new ObjectId(customerId);
            return await _orders.Find(o => o.CustomerId == objectId).ToListAsync();
        }

        public async Task UpdateOrderStatusAsync(string id, string newStatu)
        {
            UpdateDefinition<Order> update = Builders<Order>.Update.Set(o => o.Status, newStatu);
            ObjectId objectId = new ObjectId(id);
            await _orders.UpdateOneAsync(o => o.Id == objectId, update);
        }

        public async Task DeleteOrderAsync(string id)
        {
            ObjectId objectId = new ObjectId(id);
            await _orders.DeleteOneAsync(o => o.Id == objectId);
        }
    }
}
