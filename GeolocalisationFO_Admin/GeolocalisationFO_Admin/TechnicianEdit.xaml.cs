using GeolocalisationFO_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TechnicianEdit : ContentPage
    {
        private Technicien Technicien;
        private ActivityIndicator ActivityIndicator;

        public TechnicianEdit(Technicien technicien)
        {
            InitializeComponent();
            this.Technicien = technicien;
            ActivityIndicator = new ActivityIndicator();
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
            if (FieldsValidated())
            {
                try
                {
                    //instanciate the new item
                    var newTechnicien = new Technicien { Nom = TechnicianNameEntry.Text, Login = LoginEntry.Text, MotDePasse = PaswordEntry.Text };
                    //Create json object containing two items (old and new)
                    var jsonTechniciens = JsonConvert.SerializeObject(new List<Technicien> { Technicien, newTechnicien });
                    //Create a byte array of the jon object for the request
                    var bytesTechniciens = Encoding.UTF8.GetBytes(jsonTechniciens);
                    //Create the request
                    var req = WebRequest.CreateHttp("http://192.168.43.175:52640/api/GeolocalisationFO/UpdateTechnician");
                    //Set the request method to put
                    req.Method = "PUT";
                    //Set the request content-type to application/json
                    req.ContentType = "application/json";
                    //Write the data in the request stream
                    req.GetRequestStream().Write(bytesTechniciens, 0, bytesTechniciens.Length);
                    //Get the response as a string using StreamReader
                    var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
                    //Check if the response matches the expected output
                    if (resp.Equals("Technician updated Successfully"))
                    {
                        //Show confirmation alert
                        DisplayAlert("Succés", "Technicien modifiée avec succés", "Ok");
                        //Close the Edit Page => go back to All Chambers page
                        Navigation.PopAsync();
                    }
                    else
                    {
                        //The response is not as expected
                        DisplayAlert("Échec", "La modification de technicien a échoué", "ok");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    DisplayAlert("Erreur", "Verifier les valuers entrées", "Ok");
                }
            }
            else
            {
                DisplayAlert("Erreur", "Verifier les valuers entrées", "Ok");
            }
        }

        private bool FieldsValidated()
        {
            return !String.IsNullOrEmpty(TechnicianNameEntry.Text) && !String.IsNullOrEmpty(LoginEntry.Text) && !String.IsNullOrEmpty(PaswordEntry.Text);
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            Device.BeginInvokeOnMainThread(() => 
            {
                TechnicianNameEntry.Text = Technicien.Nom;
                LoginEntry.Text = Technicien.Login;
                PaswordEntry.Text = Technicien.MotDePasse;
            });
        }
    }
}