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
    public partial class AllTechnicians : ContentPage
    {
        private List<Technicien> Techniciens;
        private Technicien Technicien;

        public AllTechnicians()
        {
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();            
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            bool resp = await DisplayAlert("Confirmation", "Voulez vous supprimer cette chambre?", "Oui", "Non");
            if (resp)
            {
                var req = WebRequest.CreateHttp(Constants.DeleteTechnicianURL);
                req.Method = "DELETE";
                req.ContentType = "application/json";
                var bytesTechnicien = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Technicien));
                req.GetRequestStream().Write(bytesTechnicien, 0, bytesTechnicien.Length);
                var reqResp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
                if(reqResp.Equals("Technician deleted Successfully"))
                {
                    await DisplayAlert("Succés","Technicien supprimé avec succés","Ok");
                    Techniciens.Remove(Technicien);                    
                    await Device.InvokeOnMainThreadAsync(()=>{ 
                        DeleteButton.IsEnabled = false;
                        EditButton.IsEnabled = false;
                        CurrentTechnician.Text = "";
                        TechniciansListView.ItemsSource = new ObservableCollection<String>(Techniciens.Select(x => x.Nom)); 
                    });
                }
                else
                {
                    DisplayAlert("Échec", "Suppresion a échoué", "Ok");
                }
            }
        }

        private void EditButton_Clicked(object sender, EventArgs e)
        {
            //Disable The buttons and clear the text
            Device.BeginInvokeOnMainThread(() =>
            {
                CurrentTechnician.Text = "";
                EditButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
            });
            //Open the ChambreEdit page
            Navigation.PushAsync(new TechnicianEdit(Technicien));
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            TechniciansListView.SelectedItem = null;
            var req = WebRequest.CreateHttp(Constants.GetTechniciansURL);
            req.Method = "GET";
            var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            Techniciens = JsonConvert.DeserializeObject<List<Technicien>>(resp);
            Device.BeginInvokeOnMainThread(() =>
            {
                TechniciansListView.ItemsSource = new ObservableCollection<String>(Techniciens.Select(x => x.Nom));
            });
        }

        private void TechniciansListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (TechniciansListView.SelectedItem != null)
            {
                Technicien = Techniciens.First(x => x.Nom.Equals(TechniciansListView.SelectedItem.ToString()));
                Device.BeginInvokeOnMainThread(() =>
                {
                    CurrentTechnician.Text = $"Nom: {Technicien.Nom}\n\n\nLogin: {Technicien.Login}\n\n\nMot de passe: {Technicien.MotDePasse}";
                    DeleteButton.IsEnabled = true;
                    EditButton.IsEnabled = true;
                });
            }
        }
    }
}