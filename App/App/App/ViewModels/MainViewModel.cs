using App.Views.Popups;
using CommunityToolkit.Maui.Views;
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
    class MainViewModel : BaseViewModel
    {
        private List<Product> allProducts;

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }

        private RelayCommand addToShoppingCartCommand;
        public RelayCommand AddToShoppingCartCommand
        {
            get => addToShoppingCartCommand;
            set => SetProperty(ref addToShoppingCartCommand, value);
        }

        private RelayCommand productSelectedCommand;
        public RelayCommand ProductSelectedCommand
        {
            get => productSelectedCommand;
            set => SetProperty(ref productSelectedCommand, value);
        }

        public MainViewModel()
        {
            Products = new ObservableCollection<Product>();
            LoadProductsAsync();

            AddToShoppingCartCommand = new RelayCommand(AddToShoppingCart);
            ProductSelectedCommand = new RelayCommand(ShowProductDetails);
        }

        private async Task ShowProductDetails(object? arg)
        {
            if(arg is Product product)
            {
                ProductPopup popup = new ProductPopup(product);
                await App.Current.MainPage.ShowPopupAsync(popup);
            }
        }

        private async Task AddToShoppingCart(object? arg)
        {
            if(arg is Product product)
            {
                await Shell.Current.DisplayAlert("KOSZYK", product.Description, "OK");
                // ToDO: Zrobić obsługę dodania do koszyka
            }
        }

        private async void LoadProductsAsync()
        {
            ProductManager productManager = new ProductManager();
            allProducts = await productManager.GetAllProductsAsync();
            foreach (Product p in allProducts)
            {
                Products.Add(p);
            }
        }
    }
}
