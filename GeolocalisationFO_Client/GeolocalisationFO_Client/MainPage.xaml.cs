using GeolocalisationFO_Shared;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private Technicien Technicien;

        public MainPage(Technicien technicien)
        {
            InitializeComponent();
            Technicien = technicien;
        }

        private void MyTasksButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MyTasksPage(Technicien));
        }

        private void ChambersButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AllChambers());
        }
    }
}