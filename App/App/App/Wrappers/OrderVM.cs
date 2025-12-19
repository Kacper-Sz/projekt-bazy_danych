using dll.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Wrappers
{
    public class OrderVM
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        public DeliveryAddress DeliveryAddress { get; set; }

        public ObservableCollection<OrderItemVM> Items { get; set; } = new();
    }

    public class OrderItemVM
    {
        public string ProductName { get; set; } = "";
        public int Quantity { get; set; }
    }
}
