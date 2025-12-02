using dll.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dll.Orders
{
    public static class OrderListExtension
    {
        public static List<Order> SortOrderList(this List<Order> orders, OrderSortByEnum sortBy, bool ascending = true)
        {
            if (orders == null || orders.Count == 0)
                return orders;
            var sorted = sortBy switch
            {
                OrderSortByEnum.TOTAL_AMOUNT => ascending
                    ? orders.OrderBy(o => o.TotalAmount)
                    : orders.OrderByDescending(o => o.TotalAmount),

                OrderSortByEnum.STATUS => ascending
                    ? orders.OrderBy(o => o.Status)
                    : orders.OrderByDescending(o => o.Status),

                OrderSortByEnum.CREATED_AT => ascending
                    ? orders.OrderBy(o => o.CreatedAt)
                    : orders.OrderByDescending(o => o.CreatedAt),

                _ => orders.AsEnumerable()
            };

            return sorted.ToList();
        }
    }
}
