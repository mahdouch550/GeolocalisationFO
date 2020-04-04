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
    public partial class AddTechnician : ContentPage
    {

        public AddTechnician()
        {
            InitializeComponent();
        }

        public void AddButton_Clicked(object sender, EventArgs e)
        {
            if (FieldsValidated())
            {
                try
                {
                    var technicien = new Technicien { Nom = TechnicianNameEntry.Text, Login = LoginEntry.Text, MotDePasse = PaswordEntry.Text };
                    if (SendTechnician(technicien))
                        DisplayAlert("Succés", "Technicien ajoutée avec succés", "Ok");
                    TechnicianNameEntry.Text = "";
                    LoginEntry.Text = "";
                    PaswordEntry.Text = "";
                }
                catch (WebException exc)
                {
                    Console.WriteLine(exc.Message);
                    DisplayAlert("Erreur", "Erreur lors de l'envoi des nouvelles données", "Ok");
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

        public void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private bool SendTechnician(Technicien technicien)
        {
            var req = WebRequest.CreateHttp("http://192.168.43.175:52640/api/GeolocalisationFO/AddTechnician");
            req.Method = "POST";
            req.ContentType = "application/json";
            var jsonTechnicien = JsonConvert.SerializeObject(technicien);
            var byteArrayTechnicien = Encoding.UTF8.GetBytes(jsonTechnicien);
            req.GetRequestStream().Write(byteArrayTechnicien, 0, byteArrayTechnicien.Length);
            var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            return resp.Equals("Technician Added Successfully");
        }
    }
}