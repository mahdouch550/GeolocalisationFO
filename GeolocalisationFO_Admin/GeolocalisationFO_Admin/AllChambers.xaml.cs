using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllChambers : ContentPage
    {
        private List<Chamber> Chambers;

        public AllChambers()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();            
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool resp = await DisplayAlert("Confirmation", "Voulez vous supprimer cette chambre?", "Oui", "Non");
            if(resp)
            {
                var x = Chambers.First(y => y.Name.Equals(ChambersListView.SelectedItem.ToString()));
                Chambers.Remove(x);
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Chambers.json"), JsonConvert.SerializeObject(Chambers));
                Device.BeginInvokeOnMainThread(() => 
                { 
                    ChambersListView.ItemsSource = new ObservableCollection<String>(Chambers.Select(y => y.Name).ToList());
                });
                CurrentChamber.Text = "";
            }
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {

        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            try
            {
                var jsonStringChambers = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Chambers.json"));
                Chambers = JsonConvert.DeserializeObject<List<Chamber>>(jsonStringChambers);
            }
            catch (Exception)
            {
                Chambers = new List<Chamber>();
                File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Chambers.json"), JsonConvert.SerializeObject(Chambers));
            }
            ChambersListView.ItemsSource = new ObservableCollection<String>(Chambers.Select(x => x.Name).ToList());
        }

        private void ChambersListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var c = Chambers.FirstOrDefault(x => x.Name == ChambersListView.SelectedItem.ToString());
            CurrentChamber.Text = $"Nom: {c.Name}\n\n\n\nLongitude: {c.Longitude}\n\n\n\nLatitude: {c.Latitude}";
        }
    }
}