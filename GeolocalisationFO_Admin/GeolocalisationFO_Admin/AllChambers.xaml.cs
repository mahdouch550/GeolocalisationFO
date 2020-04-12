using GeolocalisationFO_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Admin
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
            //ChambersListView = new ListView
            //{
            //    ItemTemplate = new DataTemplate(typeof(MyCell))
            //};
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool resp = await DisplayAlert("Confirmation", "Voulez vous supprimer cette chambre?", "Oui", "Non");
            if (resp)
            {
                var req = WebRequest.CreateHttp(Constants.DeleteChamberURL);
                req.Method = "DELETE";
                req.ContentType = "application/json";
                var bytesChamber = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Chambre));
                req.GetRequestStream().Write(bytesChamber, 0, bytesChamber.Length);
                var reqResp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
                if (reqResp.Equals("Chamber deleted Successfully"))
                {
                    await DisplayAlert("Succés", "Chambre supprimée avec succés", "Ok");
                    Chambers.Remove(Chambre);
                    await Device.InvokeOnMainThreadAsync(() => {
                        DeleteButton.IsEnabled = false;
                        EditButton.IsEnabled = false;
                        CurrentChamber.Text = "";
                        ChambersListView.ItemsSource = new ObservableCollection<String>(Chambers.Select(x => x.Nom));
                    });
                }
                else
                {
                    await DisplayAlert("Échec", "Suppresion a échoué", "Ok");
                }
            }
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            //Disable The buttons and clear the text
            Device.BeginInvokeOnMainThread(() => 
            {
                CurrentChamber.Text = "";
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            });
            //Open the ChambreEdit page
            Navigation.PushAsync(new ChamberEdit(Chambre));
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
                ChambersListView.ItemsSource = new ObservableCollection<String>(Chambers.Select(x=>x.Nom));
            });
        }

        private void ChambersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (ChambersListView.SelectedItem != null)
            {
                Chambre = Chambers.FirstOrDefault(x => x.Nom == ChambersListView.SelectedItem.ToString());
                CurrentChamber.Text = $"Nom: {Chambre.Nom}\n\n\n\nLongitude: {Chambre.Longitude}\n\n\n\nLatitude: {Chambre.Latitude}";
                Device.BeginInvokeOnMainThread(() =>
                {
                    DeleteButton.IsEnabled = true;
                    EditButton.IsEnabled = true;
                });
            }
        }
    }
}