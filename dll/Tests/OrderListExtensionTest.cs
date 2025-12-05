using dll;
using dll.Models;
using dll.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class OrderListExtensionTest
    {
        private readonly OrderManager orderManager;

        public OrderListExtensionTest()
        {
            MongoDbManager dbManager = new MongoDbManager(DataManager.ConnectionString(), DataManager.DatabaseName());
            orderManager = new OrderManager(dbManager.Database);
        }

        #region SortOrderList
        #region SortByStatus
        [Fact]
        public async Task SortOrderListTest1()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("000000000000000000010000");
            List<Order> sortedOrders = orders.SortOrderList(OrderSortByEnum.STATUS, true);
            Assert.NotNull(sortedOrders);
            Assert.True(sortedOrders.Count >= 2);
            Assert.True(string.Compare(sortedOrders[0].Status, sortedOrders[1].Status) <= 0);
        }

        [Fact]
        public async Task SortOrderListTest2()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("000000000000000000010000");
            List<Order> sortedOrders = orders.SortOrderList(OrderSortByEnum.STATUS, false);
            Assert.NotNull(sortedOrders);
            Assert.True(sortedOrders.Count >= 2);
            Assert.True(string.Compare(sortedOrders[0].Status, sortedOrders[1].Status) >= 0);
        }
        #endregion
        #region SortByCreatedAt
        [Fact]
        public async Task SortOrderListTest3()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("000000000000000000010000");
            List<Order> sortedOrders = orders.SortOrderList(OrderSortByEnum.CREATED_AT, true);
            Assert.NotNull(sortedOrders);
            Assert.True(sortedOrders.Count >= 2);
            Assert.True(DateTime.Compare(sortedOrders[0].CreatedAt, sortedOrders[1].CreatedAt) <= 0);
        }

        [Fact]
        public async Task SortOrderListTest4()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("000000000000000000010000");
            List<Order> sortedOrders = orders.SortOrderList(OrderSortByEnum.CREATED_AT, false);
            Assert.NotNull(sortedOrders);
            Assert.True(sortedOrders.Count >= 2);
            Assert.True(DateTime.Compare(sortedOrders[0].CreatedAt, sortedOrders[1].CreatedAt) >= 0);
        }
        #endregion
        #region SortByTotalAmount
        [Fact]
        public async Task SortOrderListTest5()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("000000000000000000010000");
            List<Order> sortedOrders = orders.SortOrderList(OrderSortByEnum.TOTAL_AMOUNT, true);
            Assert.NotNull(sortedOrders);
            Assert.True(sortedOrders.Count >= 2);
            Assert.True(Decimal.Compare(sortedOrders[0].TotalAmount, sortedOrders[1].TotalAmount) <= 0);
        }

        [Fact]
        public async Task SortOrderListTest6()
        {
            List<Order> orders = await orderManager.GetOrdersByCustomerAsync("000000000000000000010000");
            List<Order> sortedOrders = orders.SortOrderList(OrderSortByEnum.TOTAL_AMOUNT, false);
            Assert.NotNull(sortedOrders);
            Assert.True(sortedOrders.Count >= 2);
            Assert.True(Decimal.Compare(sortedOrders[0].TotalAmount, sortedOrders[1].TotalAmount) >= 0);
        }
        #endregion
        #endregion
    }
}
