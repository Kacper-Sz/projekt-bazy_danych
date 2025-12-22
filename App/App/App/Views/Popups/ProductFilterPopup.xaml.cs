using CommunityToolkit.Maui.Views;
using System;
using System.Collections.Generic;
using dll.Models;

namespace App.Views.Popups
{
    public partial class ProductFilterPopup : Popup
    {
        public ProductFilterPopup(List<string> categories, List<string> manufacturers)
        {
            InitializeComponent();
            

            var categoriesWithAll = new List<string> { "-- Wszystkie --" };
            categoriesWithAll.AddRange(categories);
            CategoryPicker.ItemsSource = categoriesWithAll;

            var manufacturersWithAll = new List<string> { "-- Wszyscy --" };
            manufacturersWithAll.AddRange(manufacturers);
            ManufacturerPicker.ItemsSource = manufacturersWithAll;
        }

        private void OnClearClicked(object sender, EventArgs e)
        {

            SearchEntry.Text = string.Empty;
            CategoryPicker.SelectedIndex = -1;
            ManufacturerPicker.SelectedIndex = -1;
            MinPriceEntry.Text = string.Empty;
            MaxPriceEntry.Text = string.Empty;
            MinStockEntry.Text = string.Empty;


            Close(null);
        }

        private void OnApplyClicked(object sender, EventArgs e)
        {
            var result = new ProductFilterOptions();


            if (CategoryPicker.SelectedIndex > 0) // 0 to "-- Wszystkie --"
            {
                result.Category = CategoryPicker.SelectedItem as string;
            }


            if (ManufacturerPicker.SelectedIndex > 0) // 0 to "-- Wszyscy --"
            {
                result.Manufacturer = ManufacturerPicker.SelectedItem as string;
            }


            if (decimal.TryParse(MinPriceEntry.Text, out var minPrice))
                result.MinPrice = minPrice;

            if (decimal.TryParse(MaxPriceEntry.Text, out var maxPrice))
                result.MaxPrice = maxPrice;

            if (int.TryParse(MinStockEntry.Text, out var minStock))
                result.MinStock = minStock;


            result.SearchTerm = SearchEntry.Text?.Trim();

            Close(result);
        }
    }
}