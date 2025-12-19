using App.Sessions;

namespace App.Views;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();

		TestLabel.Text = $"{UserSession.CurrentUser.FirstName}: {UserSession.CurrentUser.LastName} ";
	}
}