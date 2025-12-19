using App.Sessions;
using App.Wrappers;
using dll.Models;
using dll.Orders;
using dll.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    class AccountViewModel : BaseViewModel
    {
        private ObservableCollection<OrderVM> orders;
        public ObservableCollection<OrderVM> Orders
        {
            get => orders;
            set => SetProperty(ref orders, value);
        }

        public RelayCommand LogoutCommand { get; set; }

        public AccountViewModel()
        {
            SetInitValues();
        }

        private void SetInitValues()
        {
            Orders = new ObservableCollection<OrderVM>();
            LogoutCommand = new RelayCommand(LogoutAsync);
            LoadOrdersAsync();
        }

        private async Task LogoutAsync(object? _)
        {
            Session.ClearSession();
            await Shell.Current.GoToAsync("//LoginPage");
        }

        private async void LoadOrdersAsync()
        {
            OrderManager orderManager = new OrderManager();
            ProductManager productManager = new ProductManager();

            List<Order> orders = await orderManager.GetOrdersByCustomerAsync(Session.CurrentUser.Id.ToString());
            List<Product> products = await productManager.GetAllProductsAsync();

            Orders.Clear();

            foreach (Order order in orders)
            {
                OrderVM orderVm = new OrderVM
                {
                    Id = order.Id,
                    CreatedAt = order.CreatedAt,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    PaymentMethod = order.PaymentMethod,
                    DeliveryAddress = order.DeliveryAddress
                };

                foreach (OrderItem item in order.Items)
                {
                    Product? product = products.FirstOrDefault(p => p.Id == item.ProductId);

                    orderVm.Items.Add(new OrderItemVM
                    {
                        ProductName = product?.Name ?? "Nieznany produkt",
                        Quantity = item.Quantity
                    });
                }

                Orders.Add(orderVm);
            }
        }
    }
}
