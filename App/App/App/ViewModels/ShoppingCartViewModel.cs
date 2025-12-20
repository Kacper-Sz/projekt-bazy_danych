using App.Sessions;
using App.Wrappers;
using dll;
using dll.Models;
using dll.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.ViewModels
{
    class ShoppingCartViewModel : BaseViewModel
    {
        public ObservableCollection<string> PaymentMethods { get; } 
            = new() { "Karta", "BLIK", "Przy odbiorze" };

        private string incorrectDataText;
        public string IncorrectDataText
        {
            get => incorrectDataText;
            set => SetProperty(ref incorrectDataText, value);
        }

        private bool incorrectDataIsVisible;
        public bool IncorrectDataIsVisible
        {
            get => incorrectDataIsVisible;
            set => SetProperty(ref incorrectDataIsVisible, value);
        }

        private int selectedPaymentMethod;
        public int SelectedPaymentMethod
        {
            get => selectedPaymentMethod;
            set => SetProperty(ref selectedPaymentMethod, value);
        }

        private string city;
        public string City
        {
            get => city;
            set => SetProperty(ref city, value);
        }

        private string street;
        public string Street
        {
            get => street;
            set => SetProperty(ref street, value);
        }

        private string postalCode;
        public string PostalCode
        {
            get => postalCode;
            set => SetProperty(ref postalCode, value);
        }

        public ObservableCollection<CartItem> Items { get; } = new();

        public decimal TotalAmount
            => Session.CurrentShoppingCart.TotalAmount;

        public RelayCommand RemoveItemCommand { get; set; }

        public RelayCommand IncreaseCommand { get; set; }

        public RelayCommand DecreaseCommand { get; set; }

        public RelayCommand OrderCommand { get; set; }

        public ShoppingCartViewModel()
        {
            SetInitValues();
        }

        private void SetInitValues()
        {
            RemoveItemCommand = new RelayCommand(RemoveItem);
            IncreaseCommand = new RelayCommand(Increase);
            DecreaseCommand = new RelayCommand(Decrease);
            OrderCommand = new RelayCommand(PlaceOrder);

            SelectedPaymentMethod = 0;

            IncorrectDataIsVisible = false;
            IncorrectDataText = "";
        }

        private async Task PlaceOrder(object? arg)
        {
            if (!ValidateAddress())
                return;
            if (!ValidateShoppingCart())
                return;

            DeliveryAddress address = new DeliveryAddress
            {
                City = City,
                Street = Street,
                PostalCode = PostalCode
            };

            string paymentMethod = GetPaymentMethod();

            await SubmitOrder(address, paymentMethod);
        }

        private string GetPaymentMethod()
        {
            string paymentMethod = "";
            switch (SelectedPaymentMethod)
            {
                case 0:
                    paymentMethod = "Cart";
                    break;
                case 1:
                    paymentMethod = "BLIK";
                    break;
                case 2:
                    paymentMethod = "Cash";
                    break;
                default:
                    paymentMethod = "Other";
                    break;
            }
            return paymentMethod;
        }

        private async Task SubmitOrder(DeliveryAddress address, string paymentMethod)
        {
            ProductManager productManager = new ProductManager();
            await Session.CurrentShoppingCart.SubmitAsync(productManager, Session.CurrentUser.Id, address, paymentMethod);
            OnAppearing();
        }

        private bool ValidateShoppingCart()
        {
            IncorrectDataIsVisible = false;
            IncorrectDataText = "";
            ShoppingCart? cart = Session.CurrentShoppingCart;
            if (cart == null || cart.Products.Count == 0)
            {
                IncorrectDataIsVisible = true;
                IncorrectDataText = "Koszyk jest pusty";
                return false;
            }
            return true;
        }

        private bool ValidateAddress()
        {
            IncorrectDataIsVisible = false;
            IncorrectDataText = "";
            if (string.IsNullOrWhiteSpace(City) ||
                string.IsNullOrWhiteSpace(Street) ||
                string.IsNullOrWhiteSpace(PostalCode))
            {
                IncorrectDataIsVisible = true;
                IncorrectDataText = "Podano niekompletny adres";
                return false;
            }
            return true;
        }

        private void LoadCart()
        {
            ShoppingCart? cart = Session.CurrentShoppingCart;
            if (cart == null)
                return;
            Items.Clear();
            foreach ((Product p, int count) in cart.Products)
                Items.Add(new CartItem(p, count));

            OnPropertyChanged(nameof(TotalAmount));
        }

        private async Task RemoveItem(object? arg)
        {
            if (arg is not CartItem item) 
                return;
            Session.CurrentShoppingCart?.RemoveItem(item.Product);
            Items.Remove(item);

            OnPropertyChanged(nameof(TotalAmount));
        }

        private async Task Increase(object? arg)
        {
            if (arg is not CartItem item) 
                return;
            item.Quantity++;
            Session.CurrentShoppingCart?.ChangeQuantity(item.Product, item.Quantity);

            OnPropertyChanged(nameof(TotalAmount));
        }

        private async Task Decrease(object? arg)
        {
            if (arg is not CartItem item) 
                return;
            if (item.Quantity <= 1)
            {
                RemoveItem(item);
                return;
            }
            item.Quantity--;
            Session.CurrentShoppingCart?.ChangeQuantity(item.Product, item.Quantity);

            OnPropertyChanged(nameof(TotalAmount));
        }

        public void OnAppearing()
        {
            LoadCart();
        }
    }
}