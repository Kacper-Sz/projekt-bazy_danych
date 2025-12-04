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
    }
}
