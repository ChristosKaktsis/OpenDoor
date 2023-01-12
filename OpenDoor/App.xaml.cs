using OpenDoor.Models;
using OpenDoor.Service;

namespace OpenDoor;

public partial class App : Application
{
	private static BlueToothService toothService;
	public static BlueToothService BlueToothService 
	{
		get
		{
			if(toothService == null)
				toothService = new BlueToothService();
			return toothService;
		}
	}
	public static Person Current_User { get; set; }
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
