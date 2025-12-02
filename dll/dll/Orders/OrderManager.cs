
using dll.Models;
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
        private const string COLLECTION_NAME = "Orders";
        
        private readonly IMongoCollection<Order> _orders;

        public OrderManager(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>(COLLECTION_NAME);
        }

        public async Task CreateOrderAsync(Order order)
        {
            if (!ValidateOrderData(order))
                return;
            await _orders.InsertOneAsync(order);
        }

        private bool ValidateOrderData(Order order)
            => !string.IsNullOrEmpty(order.CustomerId)
            && order.Items != null && order.Items.Count > 0
            && !string.IsNullOrEmpty(order.Status)
            && order.CreatedAt != default
            && order.DeliveryAddress != null 
            && !string.IsNullOrEmpty(order.DeliveryAddress.City)
            && !string.IsNullOrEmpty(order.DeliveryAddress.Street)
            && !string.IsNullOrEmpty(order.DeliveryAddress.PostalCode)
            && !string.IsNullOrEmpty(order.PaymentMethod);

        public async Task<Order?> GetOrderByIdAsync(string id)
            => await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();

        public async Task<List<Order>> GetOrdersByCustomerAsync(string customerId) 
            => await _orders.Find(o => o.CustomerId == customerId).ToListAsync();

        public async Task UpdateOrderStatusAsync(string id, string newStatu)
        {
            UpdateDefinition<Order> update = Builders<Order>.Update.Set(o => o.Status, newStatu);
            await _orders.UpdateOneAsync(o => o.Id == id, update);
        }

        public async Task DeleteOrderAsync(string id)
            => await _orders.DeleteOneAsync(o => o.Id == id);
    }
}
