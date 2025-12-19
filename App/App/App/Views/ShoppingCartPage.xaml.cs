using App.ViewModels;

namespace App.Views;

public partial class ShoppingCartPage : ContentPage
{
	public ShoppingCartPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ShoppingCartViewModel viewModel)
            viewModel.OnAppearing();
    }

    private void Label_Focused(object sender, FocusEventArgs e)
    {

    }
}