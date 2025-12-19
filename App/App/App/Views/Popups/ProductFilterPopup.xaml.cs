using CommunityToolkit.Maui.Views;
using dll.Models;

namespace App.Views.Popups;

public partial class ProductFilterPopup : Popup
{
    public ProductFilterOptions Filters { get; } = new ProductFilterOptions();

    public ProductFilterPopup()
	{
		InitializeComponent();

        BindingContext = Filters;
    }

	private void Apply(object? sender, EventArgs e)
		=> Close(Filters);

	private void Cancel(object? sender, EventArgs e)
		=> Close(null);
}