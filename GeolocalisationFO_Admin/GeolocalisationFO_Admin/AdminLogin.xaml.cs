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
    public partial class AdminLogin : ContentPage
    {
        public AdminLogin()
        {
            InitializeComponent();
        }

        private  void LoginButton_Clicked(object sender, EventArgs e)
        {            
            if (FieldsValidated())
            {
                LoginButton.IsEnabled = false;
                var admin = new Admin { Login = AdminLoginEntry.Text, MotDePasse = PasswordEntry.Text };
                var bytesAdmin = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(admin));
                var req = WebRequest.CreateHttp(Constants.VerifyAdminLoginURL);
                req.ContentType = "application/json";
                req.Method = "POST";
                req.GetRequestStream().Write(bytesAdmin, 0, bytesAdmin.Length);
                var result = bool.Parse(new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd());
                if (result)
                {
                     Navigation.PushAsync(new MainPage2());
                }
                else
                {
                    DisplayAlert("Erreur", "Connexion a échoué!\nVérifier Login/Mot de passe", "Ok");
                    LoginButton.IsEnabled = true;
                }
            }
            else
            {
                DisplayAlert("Erreur", "Verifier les valuers entrées", "Ok");
            }
        }

        private bool FieldsValidated()
        {
            return !String.IsNullOrEmpty(AdminLoginEntry.Text) && !String.IsNullOrEmpty(PasswordEntry.Text);
        }
    }
}