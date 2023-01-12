using OpenDoor.ViewModels;

namespace OpenDoor.Views;

public partial class PaymentPage : ContentPage
{
    private PaymentViewModel _viewModel;

    public PaymentPage()
	{
		InitializeComponent();
		BindingContext = _viewModel = new PaymentViewModel();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }
}