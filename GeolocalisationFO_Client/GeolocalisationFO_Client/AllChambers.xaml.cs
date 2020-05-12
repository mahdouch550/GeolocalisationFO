using GeolocalisationFO_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllChambers : ContentPage
    {
        private List<Chambre> Chambers;
        private Chambre Chambre;

        public AllChambers()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            ChambersListView.SelectedItem = null;
            var req = WebRequest.CreateHttp(Constants.GetChambersURL);
            req.Method = "GET";
            var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            Chambers = JsonConvert.DeserializeObject<List<Chambre>>(resp);
            Device.BeginInvokeOnMainThread(() =>
            {
                ChambersListView.ItemsSource = new ObservableCollection<String>(Chambers.Select(x => x.Nom));
            });
        }

        private void ChambersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ChambersListView.SelectedItem != null)
            {
                Chambre = Chambers.FirstOrDefault(x => x.Nom == ChambersListView.SelectedItem.ToString());
                CurrentChamber.Text = $"Nom: {Chambre.Nom}\n\n\n\nLongitude: {Chambre.Longitude}\n\n\n\nLatitude: {Chambre.Latitude}";                
            }
        }
    }
}