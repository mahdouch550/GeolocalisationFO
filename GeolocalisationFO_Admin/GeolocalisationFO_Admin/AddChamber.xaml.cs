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
    public partial class AddChamber : ContentPage
    {
        private List<Chambre> Chambers;        

        public AddChamber()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();            
        }        

        public void AddButton_Clicked(object sender, EventArgs e)
        {
            if (FieldsValidated())
            {
                try
                {
                    var chambre = new Chambre { Nom = ChamberNameEntry.Text, Longitude = float.Parse(LongitudeEntry.Text), Latitude = float.Parse(LatitudeEntry.Text) };
                    Chambers.Add(chambre);
                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Chambers.json"), JsonConvert.SerializeObject(Chambers));
                    if(SendChamber(chambre))
                        DisplayAlert("Succés", "Chambre ajoutée avec succés", "Ok");
                    ChamberNameEntry.Text = "";
                    LongitudeEntry.Text = "";
                    LatitudeEntry.Text = "";
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
            return !String.IsNullOrEmpty(ChamberNameEntry.Text) && !String.IsNullOrEmpty(LongitudeEntry.Text) && !String.IsNullOrEmpty(LatitudeEntry.Text);
        }

        public void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private bool SendChamber(Chambre chambre)
        {
            var req = WebRequest.CreateHttp("http://192.168.43.175:52640/api/GeolocalisationFO/AddChamber");
            req.Method = "POST";
            req.ContentType = "application/json";
            var jsonChambre = JsonConvert.SerializeObject(chambre);
            var byteArrayChambre = Encoding.UTF8.GetBytes(jsonChambre);
            req.GetRequestStream().Write(byteArrayChambre, 0, byteArrayChambre.Length);
            var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            return resp.Equals("Chamber Added Successfully");
        }
    }
}