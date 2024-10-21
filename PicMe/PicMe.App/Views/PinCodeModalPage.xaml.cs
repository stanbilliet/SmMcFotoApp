using PicMe.App.ViewModels;
using PicMe.Core.Interfaces.Repositories;
using PicMe.Core.Interfaces.Services;
using PicMe.Infrastructure.Repositories;


namespace PicMe.App.Views;

public partial class PinCodeModalPage : ContentPage
{
	private readonly IPinService _pinService;
	private readonly ISoapRepository _soapRepository;
	public PinCodeModalPage(IPinService pinService, ISoapRepository soapRepository)
	{
		InitializeComponent();
		_pinService = pinService;
		_soapRepository = soapRepository;
        BindingContext = new PinCodeModalViewModel(_pinService, _soapRepository);
	}
}