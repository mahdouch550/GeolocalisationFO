using GeolocalisationFO_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class AddTask : ContentPage
    {
        private List<Chambre> Chambers;
        private List<Technicien> Techniciens;

        public AddTask()
        {
            InitializeComponent();
            Chambers = new List<Chambre>();
            Techniciens = new List<Technicien>();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void ValidateButton_Clicked(object sender, EventArgs e)
        {
            if (FieldsValidated())
            {
                try
                {
                    var req = WebRequest.CreateHttp(Constants.AddTaskURL);
                    req.Method = "POST";
                    var tache = new Tache { TaskDescription = TaskDescriptionEditor.Text, ChamberID = Chambers.FirstOrDefault(x => x.Nom == ChamberPicker.SelectedItem.ToString()).Nom, TechnicianID = Techniciens.FirstOrDefault(x => x.Nom == TechnicianPicker.SelectedItem.ToString()).Login };
                    var byteArratTask = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(tache));
                    req.ContentType = "application/json";
                    req.GetRequestStream().Write(byteArratTask, 0, byteArratTask.Length);
                    var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
                    if (resp.Equals("Task Added Successfully"))
                    {
                        DisplayAlert("Succés", "Tâche ajoutée avec succés", "Ok");
                        Device.BeginInvokeOnMainThread(() => 
                        {
                            ChamberPicker.SelectedItem = null;
                            TechnicianPicker.SelectedItem = null;
                            TaskDescriptionEditor.Text = "";
                        });
                    }
                    else
                    {
                        DisplayAlert("Erreur", "Tâche non ajoutée!\nVeuillez réessayer", "Ok");
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
            return !String.IsNullOrEmpty(TechnicianPicker.SelectedItem.ToString()) && !String.IsNullOrEmpty(TaskDescriptionEditor.Text);
        }

        protected override void OnAppearing()
        {
            var req = WebRequest.CreateHttp(Constants.GetChambersURL);
            req.Method = "GET";
            var jsonChambers = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            Chambers = JsonConvert.DeserializeObject<List<Chambre>>(jsonChambers);
            req = WebRequest.CreateHttp(Constants.GetTechniciansURL);
            Techniciens = JsonConvert.DeserializeObject<List<Technicien>>(new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd());
            Device.BeginInvokeOnMainThread(() =>
            {
                TechnicianPicker.ItemsSource = new ObservableCollection<String>(Techniciens.Select(y => y.Login));
                ChamberPicker.ItemsSource = new ObservableCollection<String>(Chambers.Select(y => y.Nom));
            });
        }
    }
}