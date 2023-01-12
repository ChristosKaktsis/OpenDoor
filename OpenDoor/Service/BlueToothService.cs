using Microsoft.Maui.Controls;
using OpenDoor.Models.BlueTooth;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoor.Service
{
    public class BlueToothService
    {
        public List<DeviceCandidate> DeviceCandidateList { get; private set; }
        public IBluetoothLE BluetoothLE { get; private set; }
        public IAdapter Adapter { get; private set; }
        public IDevice Device { get; set; }
        public string Result { get; set; }

        public BlueToothService()
        {
            BluetoothLE = CrossBluetoothLE.Current;
            Adapter = CrossBluetoothLE.Current.Adapter;
            Adapter.ScanTimeout = 4000;

            Adapter.DeviceDiscovered += Adapter_DeviceDiscovered;
            Adapter.DeviceConnected += Adapter_DeviceConnected;
            Adapter.DeviceDisconnected += Adapter_DeviceDisconnected;
            Adapter.DeviceConnectionLost += Adapter_DeviceConnectionLost;

            BluetoothLE.StateChanged += BluetoothLE_StateChanged;
        }
        public string Status { get; set; }
        public string ConnectedDevice { get; set; }
        public async Task<List<DeviceCandidate>> ScanForDevicesAsync()
        {
            DeviceCandidateList = new List<DeviceCandidate>();
            try
            {
                IReadOnlyList<IDevice> systemDevices = Adapter.GetSystemConnectedOrPairedDevices();
                foreach (var systemDevice in systemDevices)
                {
                    DeviceCandidate deviceCandidate = DeviceCandidateList.FirstOrDefault(d => d.Id == systemDevice.Id);
                    if (deviceCandidate == null)
                    {
                        DeviceCandidateList.Add(new DeviceCandidate
                        {
                            Id = systemDevice.Id,
                            Name = systemDevice.Name,
                        });
                        Console.WriteLine($"Found {systemDevice.State.ToString().ToLower()} device {systemDevice.Name}.");
                    }
                }
                await Adapter.StartScanningForDevicesAsync(); //Not working ????
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to scan nearby Bluetooth LE devices: {ex.Message}.");
                await Shell.Current.DisplayAlert($"Unable to scan nearby Bluetooth LE devices", $"{ex.Message}.", "OK");
            }

            return DeviceCandidateList;
        }
        private async void StartNotify()
        {
            try
            {
                var service = await Adapter.ConnectedDevices.FirstOrDefault().GetServiceAsync(Guid.Parse("6E400001-B5A3-F393-E0A9-E50E24DCCA9E"));
                var characteristic = await service.GetCharacteristicAsync(Guid.Parse("6E400003-B5A3-F393-E0A9-E50E24DCCA9E"));

                characteristic.ValueUpdated += (o, args) =>
                {
                    var bytes = args.Characteristic.Value;
                    Result = Encoding.Default.GetString(bytes);
                };
                await characteristic.StartUpdatesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        #region DeviceEventArgs
        private async void Adapter_DeviceDiscovered(object sender, DeviceEventArgs e)
        {
            DeviceCandidate deviceCandidate = DeviceCandidateList.FirstOrDefault(d => d.Id == e.Device.Id);
            if (deviceCandidate == null)
            {
                DeviceCandidateList.Add(new DeviceCandidate
                {
                    Id = e.Device.Id,
                    Name = e.Device.Name,
                });
                await ShowToastAsync($"Found {e.Device.State.ToString().ToLower()} {e.Device.Name}.");
            }
        }

        private void Adapter_DeviceConnectionLost(object sender, DeviceErrorEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await ShowToastAsync($"{e.Device.Name} connection is lost.");
                }
                catch
                {
                    await ShowToastAsync($"Device connection is lost.");
                }
            });
        }

        private void Adapter_DeviceConnected(object sender, DeviceEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await ShowToastAsync($"{e.Device.Name} is connected.");
                }
                catch
                {
                    await ShowToastAsync($"Device is connected.");
                }
            });
        }

        private void Adapter_DeviceDisconnected(object sender, DeviceEventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await ShowToastAsync($"{e.Device.Name} is disconnected.");
                }
                catch
                {
                    await ShowToastAsync($"Device is disconnected.");
                }
            });
        }
        #endregion DeviceEventArgs

        #region BluetoothStateChangedArgs
        private void BluetoothLE_StateChanged(object sender, BluetoothStateChangedArgs e)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    await ShowToastAsync($"Bluetooth state is {e.NewState}.");
                }
                catch
                {
                    await ShowToastAsync($"Bluetooth state has changed.");
                }
            });
        }
        #endregion BluetoothStateChangedArgs
#if ANDROID
        #region BluetoothPermissions
        public async Task<PermissionStatus> CheckBluetoothPermissions()
        {
            PermissionStatus status = PermissionStatus.Unknown;
            try
            {
                status = await Permissions.CheckStatusAsync<BluetoothLEPermissions>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to check Bluetooth LE permissions: {ex.Message}.");
                await Shell.Current.DisplayAlert($"Unable to check Bluetooth LE permissions", $"{ex.Message}.", "OK");
            }
            return status;
        }

        public async Task<PermissionStatus> RequestBluetoothPermissions()
        {
            PermissionStatus status = PermissionStatus.Unknown;
            try
            {
                status = await Permissions.RequestAsync<BluetoothLEPermissions>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to request Bluetooth LE permissions: {ex.Message}.");
                await Shell.Current.DisplayAlert($"Unable to request Bluetooth LE permissions", $"{ex.Message}.", "OK");
            }
            return status;
        }
        #endregion BluetoothPermissions
#elif IOS
#elif WINDOWS
#endif
        public async Task ShowToastAsync(string message)
        {
            //await Shell.Current.DisplayAlert($"Bluetooth LE devices", $"{message}.", "OK");
        }
        
    }
}
