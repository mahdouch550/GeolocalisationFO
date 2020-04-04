using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeolocalisationFO_Admin
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(true)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

        public void AddChamber_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddChamber());
        }

        public void AllChambers_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AllChambers());
        }

        public void AddTechnician_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTechnician());
        }

        public void AllTechnicians_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AllTechnicians());
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}