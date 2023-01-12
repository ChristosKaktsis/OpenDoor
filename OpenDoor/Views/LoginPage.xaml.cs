using OpenDoor.ViewModels;

namespace OpenDoor.Views;

public partial class LoginPage : ContentPage
{
    private LoginViewModel _viewModel;

    public LoginPage()
	{
		InitializeComponent();
		BindingContext = _viewModel = new LoginViewModel();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }
}