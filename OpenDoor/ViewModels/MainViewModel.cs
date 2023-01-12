using OpenDoor.Service;
using OpenDoor.Uuids;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OpenDoor.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ImageSource _Image = "lock.png";
        private string _Status = "Locked";
        private readonly DateTime isNow = DateTime.Now;
        public MainViewModel()
        {
            ChangeCommand = new Command(ChangeStatus);
            InitializeService();
        }

        private async void InitializeService()
        {
            try
            {
                if (App.BlueToothService.Device.State != Plugin.BLE.Abstractions.DeviceState.Connected) return;

                var service = await App.BlueToothService.Device.GetServiceAsync(BlueToothUuids.LedService);
                if(service == null) return;
                var characteristic = await service.GetCharacteristicAsync(BlueToothUuids.LedCharacteristic);
                if(characteristic == null) return;
                characteristic.ValueUpdated += Characteristic_ValueUpdated;
                await characteristic.StartUpdatesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void Characteristic_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
        {
            var bytes = e.Characteristic.Value;
            Result = Encoding.Default.GetString(bytes);
        }

        private async void ChangeStatus()
        {
            if (!IsAllowed())
            {
                await Shell.Current
                    .DisplayAlert(
                    $"Entrance is not allowed",
                    $"User {App.Current_User.FirstName} is not on current schedule",
                    "OK");
                return;
            }
            await UpdateAttendance();

            if (IsLocked) return;//del this after 

            IsLocked = !IsLocked;
            await TurnONOFF(IsLocked);
            // this should be written on the ble device
            await Task.Delay(3000); // delete this after production
            IsLocked = !IsLocked;
            await TurnONOFF(IsLocked);
        }
        private async Task UpdateAttendance()
        {
            if(IsBusy) return;
            IsBusy = true;
            try
            {
                var item = App.Current_User.Attendances
                    .Where(a => a.StartOn.Date == isNow.Date).FirstOrDefault();
                var result = await AttendanceApi.UpdateItemAsync(item);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                IsBusy = false;
            }
        }
        private bool IsAllowed()
        {
            try
            {
                var cur = App.Current_User.Attendances
                    .Where(a => a.StartOn.Date == isNow.Date).Any();
                if(!cur) return false;
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return false;
            }
        }
        private async Task TurnONOFF(bool isToOpen)
        {
            try
            {
                if (App.BlueToothService.Device.State != Plugin.BLE.Abstractions.DeviceState.Connected) return;

                var service = await App.BlueToothService.Device.GetServiceAsync(BlueToothUuids.LedService);
                if (service == null) return;
                var characteristic = await service.GetCharacteristicAsync(BlueToothUuids.WriteCharacteristic);
                if (characteristic == null) return;
                var bytes = Encoding.UTF8.GetBytes(isToOpen ? "1" : "0");//0 to turn off 1 to turn on
                await characteristic.WriteAsync(bytes);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public string Result
        {
            set
            {
                Status = value;
                if (value == "ON")
                {
                    Image = "unlock.png";
                    IsLocked = true;
                }
                else if (value == "OFF")
                {
                    Image = "lock.png";
                    IsLocked = false;
                }
            }
        }
        public ImageSource Image 
        {
            get => _Image; 
            set => SetProperty(ref _Image , value); 
        } 
        public string Status 
        { 
            get => _Status; 
            set=> SetProperty(ref _Status, value); 
        } 
        public bool IsLocked { get; set; } = true;
        public Command ChangeCommand { get; }

    }
}
