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
    public partial class ChamberEdit : ContentPage
    {
        private List<Chambre> Chambers;
        private Chambre Chamber;

        public ChamberEdit(Chambre Chamber)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            this.Chamber = Chamber;            
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
            //Verify fields are valid
            if (FieldsValidated())
            {
                try
                {
                    //instanciate the new item
                    var newChamber = new Chambre { Nom = ChamberNameEntry.Text, Longitude = float.Parse(LongitudeEntry.Text), Latitude = float.Parse(LatitudeEntry.Text) };
                    //Create json object containing two items (old and new)
                    var jsonChambers = JsonConvert.SerializeObject(new List<Chambre> { Chamber, newChamber });
                    //Create a byte array of the jon object for the request
                    var bytesChambers = Encoding.UTF8.GetBytes(jsonChambers);
                    //Create the request
                    var req = WebRequest.CreateHttp(Constants.UpdateChamberURL);
                    //Set the request method to put
                    req.Method = "PUT";
                    //Set the request content-type to application/json
                    req.ContentType = "application/json";
                    //Write the data in the request stream
                    req.GetRequestStream().Write(bytesChambers, 0, bytesChambers.Length);
                    //Get the response as a string using StreamReader
                    var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
                    //Check if the response matches the expected output
                    if (resp.Equals("Chamber updated Successfully"))
                    {
                        //Show confirmation alert
                        DisplayAlert("Succés","Chambre modifiée avec succés","Ok");
                        //Close the Edit Page => go back to All Chambers page
                        Navigation.PopAsync();
                    }
                    else
                    {
                        //The response is not as expected
                        DisplayAlert("Échec", "La modification de chambre a échoué", "ok");
                    }
                }
                catch(Exception ex)
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

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        //Add the override OnAppearing method
        protected override void OnAppearing()
        {
            //Fill he form with the selected item
            Device.BeginInvokeOnMainThread(() => 
            {
                ChamberNameEntry.Text = Chamber.Nom;
                LongitudeEntry.Text = Chamber.Longitude.ToString();
                LatitudeEntry.Text = Chamber.Latitude.ToString();
                //At this moment, the form is ready to be changed
            });
        }         
    }
}