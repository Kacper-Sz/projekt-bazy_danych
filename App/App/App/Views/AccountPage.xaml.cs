using App.ViewModels;

namespace App.Views;

public partial class AccountPage : ContentPage
{
	public AccountPage()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AccountViewModel viewModel)
            viewModel.OnAppearing();
    }
}