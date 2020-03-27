using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChamberEdit : ContentPage
    {
        private List<Chamber> Chambers;
        private Chamber Chamber;

        public ChamberEdit(Chamber Chamber)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            this.Chamber = Chamber;
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
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
            Chambers.Remove(Chamber);
            if (FieldsValidated())
            {
                try
                {
                    Chambers.Add(new Chamber { Name = ChamberNameEntry.Text, Longitude = float.Parse(LongitudeEntry.Text), Latitude = float.Parse(LatitudeEntry.Text) });
                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Chambers.json"), JsonConvert.SerializeObject(Chambers));
                    DisplayAlert("Succés", "Chambre modifiée avec succés", "Ok");
                    Navigation.PopAsync();
                }
                catch
                {
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
    }
}