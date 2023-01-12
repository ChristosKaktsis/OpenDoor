using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using OpenDoor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenDoor.Templates
{
    public class PaymentItemModel
    {
        public Responsibility Responsibility { get; set; }
        public Command CheckoutCommand
        { get; } = new Command(async () => await Shell.Current.DisplayAlert("ΠΛηρωμή", "Πληρωμή", "Οκ"));
        public bool IsExpired { get => Responsibility.Date < DateTime.Now; }
        public Color BackgroundColor 
        {
            get => IsExpired? Color.FromArgb("#cc0000"): Color.FromArgb("#ffffff");
         }
    }
}
