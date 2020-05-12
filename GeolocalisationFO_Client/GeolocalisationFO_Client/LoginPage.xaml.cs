using GeolocalisationFO_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GeolocalisationFO_Client
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {
        private Technicien CurrentTechnician;

        public LoginPage()
        {
            InitializeComponent();
        }

        private bool FieldsValidated()
        {
            return !String.IsNullOrEmpty(TechnicianLoginEntry.Text) && !String.IsNullOrEmpty(PasswordEntry.Text);
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (FieldsValidated())
            {
                LoginButton.IsEnabled = false;
                var admin = new Technicien { Login = TechnicianLoginEntry.Text, MotDePasse = PasswordEntry.Text };
                var bytesAdmin = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(admin));
                var req = WebRequest.CreateHttp(Constants.VerifyTechnicianLoginURL);
                req.ContentType = "application/json";
                req.Method = "POST";
                await req.GetRequestStream().WriteAsync(bytesAdmin, 0, bytesAdmin.Length);
                var result = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
                if (result.Equals("null"))
                {
                    await DisplayAlert("Échéc de login", "Votre Login/Mot de passe est incorrect", "Ok");
                    return;
                }
                CurrentTechnician = JsonConvert.DeserializeObject<Technicien>(result);
                await Navigation.PushAsync(new MainPage(CurrentTechnician));
            }
            else
            {
                await DisplayAlert("Erreur", "Verifier les valuers entrées", "Ok");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}