using dll;
using dll.Models;
using dll.Orders;
using dll.Users;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class OrderManagerTests
    {
        private readonly OrderManager orderManager;

        public OrderManagerTests()
        {
            MongoDbManager dbManager = new MongoDbManager(DataManager.ConnectionString(), DataManager.DatabaseName());
            orderManager = new OrderManager(dbManager.Database);
        }

        #region CreateOrderAsync
        #region InvalidData
        [Fact]
        public async Task Test1CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId(),
                Items = new List<OrderItem>()
                {
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f49"), Quantity = 2 },
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f4a"), Quantity = 1 }
                },
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "Warsaw",
                    Street = "Main Street 1",
                    PostalCode = "00-001"
                },
                PaymentMethod = "Credit Card"
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.INVALID_DATA, result);
        }

        [Fact]
        public async Task Test2CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId("6931a62442ebb44d99ce5f79"),
                Items = new List<OrderItem>(),
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "Warsaw",
                    Street = "Main Street 1",
                    PostalCode = "00-001"
                },
                PaymentMethod = "Credit Card"
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.INVALID_DATA, result);
        }

        [Fact]
        public async Task Test3CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId("6931a62442ebb44d99ce5f79"),
                Items = new List<OrderItem>()
                {
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f49"), Quantity = 2 },
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f4a"), Quantity = 1 }
                },
                Status = "",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "Warsaw",
                    Street = "Main Street 1",
                    PostalCode = "00-001"
                },
                PaymentMethod = "Credit Card"
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.INVALID_DATA, result);
        }

        [Fact]
        public async Task Test4CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId("6931a62442ebb44d99ce5f79"),
                Items = new List<OrderItem>()
                {
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f49"), Quantity = 2 },
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f4a"), Quantity = 1 }
                },
                Status = "Processing",
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "Warsaw",
                    Street = "Main Street 1",
                    PostalCode = "00-001"
                },
                PaymentMethod = "Credit Card"
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.INVALID_DATA, result);
        }

        [Fact]
        public async Task Test5CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId("6931a62442ebb44d99ce5f79"),
                Items = new List<OrderItem>()
                {
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f49"), Quantity = 2 },
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f4a"), Quantity = 1 }
                },
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "",
                    Street = "Main Street 1",
                    PostalCode = "00-001"
                },
                PaymentMethod = "Credit Card"
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.INVALID_DATA, result);
        }

        [Fact]
        public async Task Test6CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId("6931a62442ebb44d99ce5f79"),
                Items = new List<OrderItem>()
                {
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f49"), Quantity = 2 },
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f4a"), Quantity = 1 }
                },
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "Warsaw",
                    Street = "",
                    PostalCode = "00-001"
                },
                PaymentMethod = "Credit Card"
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.INVALID_DATA, result);
        }

        [Fact]
        public async Task Test7CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId("6931a62442ebb44d99ce5f79"),
                Items = new List<OrderItem>()
                {
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f49"), Quantity = 2 },
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f4a"), Quantity = 1 }
                },
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "Warsaw",
                    Street = "Main Street 1",
                    PostalCode = ""
                },
                PaymentMethod = "Credit Card"
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.INVALID_DATA, result);
        }

        [Fact]
        public async Task Test8CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId("6931a62442ebb44d99ce5f79"),
                Items = new List<OrderItem>()
                {
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f49"), Quantity = 2 },
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f4a"), Quantity = 1 }
                },
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "Warsaw",
                    Street = "Main Street 1",
                    PostalCode = "00-001"
                },
                PaymentMethod = ""
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.INVALID_DATA, result);
        }
        #endregion

        #region Success
        [Fact]
        public async Task Test9CreateOrderAsync()
        {
            Order newOrder = new Order()
            {
                CustomerId = new ObjectId("6931a62442ebb44d99ce5f7c"),
                Items = new List<OrderItem>()
                {
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f49"), Quantity = 2 },
                    new OrderItem() { ProductId = new ObjectId("6931a62442ebb44d99ce5f4a"), Quantity = 1 }
                },
                Status = "Processing",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = 12000,
                DeliveryAddress = new DeliveryAddress()
                {
                    City = "Warsaw",
                    Street = "Main Street 1",
                    PostalCode = "00-001"
                },
                PaymentMethod = "Credit Card"
            };
            OrderCreationEnum result = await orderManager.CreateOrderAsync(newOrder);
            Assert.Equal(OrderCreationEnum.SUCCESS, result);
        }
        #endregion
        #endregion

        #region GetOrderByIdAsync
        #region InvalidId
        [Fact]
        public async Task Test1GetOrderByIdAsync()
        {
            Order? order = await orderManager.GetOrderByIdAsync("000000000000000000000000");
            Assert.Null(order);
        }
        #endregion

        #region CorrectId
        [Fact]
        public async Task Test2GetOrderByIdAsync()
        {
            Order? order = await orderManager.GetOrderByIdAsync("6931a4613e1b605c1bce5f7d");
            Assert.NotNull(order);
        }
        #endregion
        #endregion

        #region GetOrdersByCustomerAsync
        #region InvalidCustomerId
        [Fact]
        public async Task Test1GetOrdersByCustomerAsync()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("000000000000000000000001");
            Assert.Empty(orders);
        }
        #endregion

        #region CorrectCustomerIdNoOrders
        [Fact]
        public async Task Test2GetOrdersByCustomerAsync()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("6931a62442ebb44d99ce5f7b");
            Assert.Empty(orders);
        }
        #endregion

        #region CorrectCustomerIdWithOrders
        [Fact]
        public async Task Test3GetOrdersByCustomerAsync()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("6931a62442ebb44d99ce5f79");
            Assert.NotEmpty(orders);
            Assert.Equal(2, orders.Count);
        }
        #endregion
        #endregion

        #region UpdateOrderStatusAsync
        #region CorrectId
        [Fact]
        public async Task Test1UpdateOrderStatusAsync()
        {
            Order? orderBefore = await orderManager.GetOrderByIdAsync("000000000000000200000000");
            Assert.NotNull(orderBefore);
            await orderManager.UpdateOrderStatusAsync("000000000000000200000000", "Shipped");
            Order? order = await orderManager.GetOrderByIdAsync("000000000000000200000000");
            Assert.NotNull(order);
            Assert.Equal("Shipped", order.Status);
        }
        #endregion
        #endregion

        #region DeleteOrderAsync
        #region CorrectId
        [Fact]
        public async Task Test1DeleteOrderAsync()
        {
            Order? orderBefore = await orderManager.GetOrderByIdAsync("000000000000000300000000");
            Assert.NotNull(orderBefore);
            await orderManager.DeleteOrderAsync("000000000000000300000000");
            Order? orderAfter = await orderManager.GetOrderByIdAsync("000000000000000300000000");
            Assert.Null(orderAfter);
        }
        #endregion
        #endregion
    }
}
