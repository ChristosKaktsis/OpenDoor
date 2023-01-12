using OpenDoor.Models;
using OpenDoor.Models.BlueTooth;
using OpenDoor.Service;
using OpenDoor.Views;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoor.ViewModels
{
    internal class LoginViewModel:BaseViewModel
    {
        private bool _IsScanning;

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLogin);
            ConnectCommand = new Command(async () => await ConnectToESP32());
        }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsScanning
        {
            get => _IsScanning;
            set => SetProperty(ref _IsScanning, value);
        }
        public List<DeviceCandidate> Devices { get; set; } = new List<DeviceCandidate>();
        public async void OnAppearing()
        {
            await Task.Delay(1000);
            await ScanForDevicesAsync();
            await ConnectToESP32();
        }

        private async Task ScanForDevicesAsync()
        {
            if (IsScanning) return;

            if (!App.BlueToothService.BluetoothLE.IsAvailable)
            {
                Debug.WriteLine($"Bluetooth is missing.");
                await Shell.Current.DisplayAlert($"Bluetooth", $"Bluetooth is missing.", "OK");
                return;
            }
#if ANDROID
        PermissionStatus permissionStatus = await App.BlueToothService.CheckBluetoothPermissions();
        if (permissionStatus != PermissionStatus.Granted)
        {
            permissionStatus = await App.BlueToothService.RequestBluetoothPermissions();
            if (permissionStatus != PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert($"Bluetooth LE permissions", $"Bluetooth LE permissions are not granted.", "OK");
                return;
            }
        }
#elif IOS
#elif WINDOWS
#endif

            try
            {
                if (!App.BlueToothService.BluetoothLE.IsOn)
                {
                    await Shell.Current.DisplayAlert(
                        $"Bluetooth is not on", 
                        $"Please turn Bluetooth on and try again.", 
                        "OK");
                    return;
                }
                IsScanning = true;
                var devices = await App.BlueToothService.ScanForDevicesAsync(); 
                if( devices.Count == 0)
                    await Shell.Current.DisplayAlert(
                        $"Bluetooth", 
                        $"Unable to find nearby Bluetooth LE devices. Try again.", 
                        "OK");
                Devices.Clear();
                foreach(var bdevice in devices)
                    Devices.Add(bdevice);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                await Shell.Current.DisplayAlert(
                    $"Unable to get nearby Bluetooth LE devices", 
                    $"{e.Message}.", "OK");
            }
            finally
            {
                IsScanning = false;
            }
        }

        private async void OnLogin(object obj)
        {
            if (IsScanning) return;
            var user = await GetUser(UserName, Password);
            if (user == null)
            {
                await Shell.Current.DisplayAlert(
                    $"User error",
                    $"User Not Found ", "OK");
                return;
            }
            App.Current_User = user;
            if (App.BlueToothService.Device == null) return;
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }
        private async Task ConnectToESP32()
        {
            if (IsScanning) return;

            try
            {
                IsScanning = true;
                foreach (var device in Devices)
                {
                    if (device.Name == "ESP32")
                        App.BlueToothService.Device = await App.BlueToothService
                            .Adapter.ConnectToKnownDeviceAsync(device.Id);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
                await Shell.Current.DisplayAlert(
                    $"Bluetooth ESP32", 
                    $"Unable to connect to ESP32", 
                    "OK");
            }
            finally { IsScanning = false; }
        }
        private async Task<Person> GetUser(string userName, string password)
        {
            try
            {
                return await PersonApi.GetItemAsync(userName);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
            finally
            {

            }
        }
        public Command LoginCommand { get; }
        public Command ConnectCommand { get; set; }
    }
}
