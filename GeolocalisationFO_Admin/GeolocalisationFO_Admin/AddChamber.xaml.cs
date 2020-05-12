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
                    var chamberInsertResult = SendChamber(chambre);


                    if (chamberInsertResult.Equals("Chamber Added Successfully"))
                    {
                        DisplayAlert("Succés", "Chambre ajoutée avec succés", "Ok");
                        ChamberNameEntry.Text = "";
                        LongitudeEntry.Text = "";
                        LatitudeEntry.Text = "";
                    }

                    if (chamberInsertResult.Equals("Chamber Name exists in the database"))
                    {
                        DisplayAlert("Érreur", "Chambre de même nom existe déja", "Ok");                        
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
            return !String.IsNullOrEmpty(ChamberNameEntry.Text) && !String.IsNullOrEmpty(LongitudeEntry.Text) && !String.IsNullOrEmpty(LatitudeEntry.Text);
        }

        public void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private String SendChamber(Chambre chambre)
        {
            var req = WebRequest.CreateHttp(Constants.AddChamberURL);
            req.Method = "POST";
            req.ContentType = "application/json";
            var jsonChambre = JsonConvert.SerializeObject(chambre);
            var byteArrayChambre = Encoding.UTF8.GetBytes(jsonChambre);
            req.GetRequestStream().Write(byteArrayChambre, 0, byteArrayChambre.Length);
            var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();            
            return resp;
        }
    }
}