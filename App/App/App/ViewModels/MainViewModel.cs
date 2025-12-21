using App.Sessions;
using App.Views.Popups;
using CommunityToolkit.Maui.Views;
using dll;
using dll.Models;
using dll.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private List<Product> allProducts;

        private ProductFilterOptions? _productFilterOptions;

        private ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set => SetProperty(ref products, value);
        }

        public ObservableCollection<string> SortingTypes { get; } 
            = new ObservableCollection<string>()
            {
                "Domyślnie",
                "Nazwa (rosnąco)",
                "Nazwa (malejąco)",
                "Kategoria (rosnąco)",
                "Kategoria (malejąco)",
                "Ilość (rosnąco)",
                "Ilość (malejąco)",
                "Cena (rosnąco)",
                "Cena (malejąco)",
                "Data (rosnąco)",
                "Data (malejąco)",
            };

        private int selectedSorting;
        public int SelectedSorting
        {
            get => selectedSorting;
            set
            {
                SetProperty(ref selectedSorting, value);
                LoadProductsAsync();
            }
        }

        public RelayCommand AddToShoppingCartCommand { get; set; }

        public RelayCommand ProductSelectedCommand { get; set; }

        public RelayCommand OpenFilterCommand { get; set; }

        public MainViewModel()
        {
            SetInitValues();
        }

        private void SetInitValues()
        {
            Session.CurrentShoppingCart = new ShoppingCart();

            Products = new ObservableCollection<Product>();
            LoadProductsAsync();

            AddToShoppingCartCommand = new RelayCommand(AddToShoppingCart);
            ProductSelectedCommand = new RelayCommand(ShowProductDetails);
            OpenFilterCommand = new RelayCommand(OpenFilter);
            SelectedSorting = 0;
        }

        private async Task OpenFilter(object? _)
        {
            ProductFilterPopup popup = new ProductFilterPopup();
            object? filterOptions = await App.Current.MainPage.ShowPopupAsync(popup);
            if (filterOptions is ProductFilterOptions filters)
                _productFilterOptions = filters;
            else
                _productFilterOptions = null;
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            List<Product> productsToFilter = allProducts.ToList();
            if (_productFilterOptions != null)
            {               
                if (!string.IsNullOrEmpty(_productFilterOptions.Category))
                    productsToFilter = productsToFilter.FilterByCategory(_productFilterOptions.Category);
                if (!string.IsNullOrWhiteSpace(_productFilterOptions.Manufacturer))
                    productsToFilter = productsToFilter.FilterByManufacturer(_productFilterOptions.Manufacturer);
                if (_productFilterOptions.MinStock.HasValue)
                    productsToFilter = productsToFilter.FilterByStock(_productFilterOptions.MinStock.Value);
                if (!string.IsNullOrWhiteSpace(_productFilterOptions.SearchTerm))
                    productsToFilter = productsToFilter.SearchByName(_productFilterOptions.SearchTerm);
                productsToFilter = FilterByPrice(productsToFilter);
            }
            Products.Clear();
            foreach (Product p in productsToFilter)
                Products.Add(p);
        }

        private List<Product> FilterByPrice(List<Product> productsToFilter)
        {
            if (!_productFilterOptions.MinPrice.HasValue)
                _productFilterOptions.MinPrice = 0;
            if (!_productFilterOptions.MaxPrice.HasValue)
                _productFilterOptions.MaxPrice = decimal.MaxValue;
            return productsToFilter.FilterByPriceRange(_productFilterOptions.MinPrice.Value, _productFilterOptions.MaxPrice.Value);
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
                Session.CurrentShoppingCart.AddItem(product, 1);
                await RefreshAllProducts();
            }
        }

        private async void LoadProductsAsync()
        {
            await RefreshAllProducts();
            Products.Clear();
            ApplyFilters();
        }

        private async Task RefreshAllProducts()
        {
            ProductManager productManager = new ProductManager();
            allProducts = await productManager.GetAllProductsAsync();
            switch (SelectedSorting)
            {
                case 1:
                    allProducts = allProducts.SortProducts(ProductSortEnum.NAME, true);
                    break;
                case 2:
                    allProducts = allProducts.SortProducts(ProductSortEnum.NAME, false);
                    break;
                case 3:
                    allProducts = allProducts.SortProducts(ProductSortEnum.CATEGORY, true);
                    break;
                case 4:
                    allProducts = allProducts.SortProducts(ProductSortEnum.CATEGORY, false);
                    break;
                case 5:
                    allProducts = allProducts.SortProducts(ProductSortEnum.STOCK, true);
                    break;
                case 6:
                    allProducts = allProducts.SortProducts(ProductSortEnum.STOCK, false);
                    break;
                case 7:
                    allProducts = allProducts.SortProducts(ProductSortEnum.PRICE, true);
                    break;
                case 8:
                    allProducts = allProducts.SortProducts(ProductSortEnum.PRICE, false);
                    break;
                case 9:
                    allProducts = allProducts.SortProducts(ProductSortEnum.CREATED_AT, true);
                    break;
                case 10:
                    allProducts = allProducts.SortProducts(ProductSortEnum.CREATED_AT, false);
                    break;
            }
        }
    }
}
