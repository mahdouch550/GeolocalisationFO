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
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Client
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyTasksPage : ContentPage
    {
        private Technicien Technicien;
        private List<Chambre> Chambres;
        private List<Tache> Taches;
        private Map MapView;

        public MyTasksPage(Technicien technicien)
        {
            InitializeComponent();
            this.Technicien = technicien;
            Position position = new Position(34.741094, 10.752437);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            MapView = new Map(mapSpan) { MapType = MapType.Satellite };
            MainGrid.Children.AddVertical(MapView);
        }

        protected override void OnAppearing()
        {
            var request = WebRequest.CreateHttp(Constants.GetMyTasksURL + "?TechnicianLogin=" + Technicien.Login);
            request.Method = "GET";
            request.ContentType = "application/json";
            var stringResponse = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            Taches = JsonConvert.DeserializeObject<List<Tache>>(stringResponse);
            Device.BeginInvokeOnMainThread(() =>
            {
                MyTasksListView.ItemsSource = new ObservableCollection<String>(Taches.Select(x => $"{x.ID} - {x.TechnicianLogin} - {x.ChamberID}"));
            });
            request = WebRequest.CreateHttp(Constants.GetChambersURL);
            request.Method = "GET";
            request.ContentType = "application/json";
            stringResponse = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            Chambres = JsonConvert.DeserializeObject<List<Chambre>>(stringResponse);
        }

        private void MarkFinishedButton_Clicked(object sender, EventArgs e)
        {
            var selectedTaskId = MyTasksListView.SelectedItem.ToString().Split(' ')[0];
            var selectedTask = Taches.FirstOrDefault(x => x.ID.ToString().Equals(selectedTaskId));
            var request = WebRequest.CreateHttp(Constants.UpdateTaskURL);
            request.Method = "PUT";
            request.ContentType = "application/json";
            var byteArrayTask = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(selectedTask));
            request.GetRequestStream().Write(byteArrayTask, 0, byteArrayTask.Length);
            var resp = new StreamReader(request.GetResponse().GetResponseStream()).ReadToEnd();
            if (resp.Equals("Task updated Successfully"))
            {
                DisplayAlert("Succés", "Tâche marquée finie!", "Ok");
            }
            if (Taches.Count == 0)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MarkFinishedButton.IsEnabled = false;
                });
                return;
            }
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void MyTasksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedTaskId = MyTasksListView.SelectedItem.ToString().Split(' ')[0];

            var selectedTask = Taches.ElementAt(e.SelectedItemIndex);
            var selectedTaskChamber = Chambres.FirstOrDefault(x => x.Nom.Equals(selectedTask.ChamberID));
            var newPosition = new Position(longitude: selectedTaskChamber.Latitude, latitude: selectedTaskChamber.Longitude);
            Pin pin = new Pin
            {
                Label = selectedTaskChamber.Nom,
                Address = selectedTask.TaskDescription,
                Type = PinType.Place,
                Position = newPosition                
            };
            var ms = MapSpan.FromCenterAndRadius(newPosition, Distance.FromKilometers(0.444));
            Device.BeginInvokeOnMainThread(() =>
            {
                MapView.Pins.Clear();
                if (selectedTask.TaskFinished)
                {
                    MarkFinishedButton.IsEnabled = false;
                    pin.Type = PinType.Generic;

                }
                else
                {
                    MarkFinishedButton.IsEnabled = true;
                    pin.Type = PinType.Place;
                }
                MapView.Pins.Add(pin);
                MapView.MoveToRegion(ms);
                TaskDetailsLabel.Text = $"ID: {selectedTask.ChamberID}\n\nTechnicien: {selectedTask.TechnicianLogin}\n\nChambre: {selectedTask.ChamberID}";
                var status = selectedTask.TaskFinished ? "Finie" : "Non Finie";
                TaskDescriptionLabel.Text = "Tâche:\n"+ selectedTask.TaskDescription+$"\nStatus: {status}";
            });
        }
    }
}