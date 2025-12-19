using CommunityToolkit.Maui.Views;
using dll.Models;
using System.Collections.ObjectModel;

namespace App.Views.Popups;

public partial class ProductPopup : Popup
{
	public ObservableCollection<string> SpecsList { get; } = new ObservableCollection<string>();

    public string ProductName { get; }
    public string ProductImageUrl { get; }
    public string Manufacturer { get; }
    public string Category { get; }
    public decimal Price { get; }
    public int Stock { get; }
    public string Description { get; }


    public ProductPopup(Product product)
	{
		InitializeComponent();

        ProductName = product.Name;
        ProductImageUrl = product.ImageUrl;
        Manufacturer = product.Manufacturer;
        Category = product.Category;
        Price = product.Price;
        Stock = product.Stock;
        Description = product.Description;

        foreach ((string key, string value) in product.Specs)
            SpecsList.Add($"{key}: {value}");

        BindingContext = this;
    }

	private void ClosePopup(object sender, EventArgs e)
		=> Close();
}