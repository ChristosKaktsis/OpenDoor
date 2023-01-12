using OpenDoor.Models;
using OpenDoor.Service;
using OpenDoor.Templates;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoor.ViewModels
{
    public class PaymentViewModel : BaseViewModel
    {
        public ObservableCollection<PaymentItemModel> Responsibilities
        { get; set; } = new ObservableCollection<PaymentItemModel>();
        public Command LoadCommand { get;}
        public PaymentViewModel() 
        { 
            LoadCommand = new Command(async () => await LoadResponsibilities());
        }
        public void OnAppearing()
        {
            IsBusy = true;
        }
        private async Task LoadResponsibilities()
        {
            IsBusy = true;
            try
            {
                var items = await ResponsibilityApi.GetItemsAsync(App.Current_User.CodeNo);
                Responsibilities.Clear();
                foreach (var item in items)
                    Responsibilities.Add(new PaymentItemModel { Responsibility = item });
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex);
                await Shell.Current.DisplayAlert("Load items", "Failed to load items", "OK");
            }
            finally { IsBusy = false; }
        }
    }
}
