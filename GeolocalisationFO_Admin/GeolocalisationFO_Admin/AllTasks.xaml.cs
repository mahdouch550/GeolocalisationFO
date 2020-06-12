using GeolocalisationFO_Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllTasks : ContentPage
    {
        private List<Tache> Taches;
        private Tache Tache;

        public AllTasks()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            TasksListView.SelectedItem = null;
            var req = WebRequest.CreateHttp(Constants.GetAllTasksURL);
            req.Method = "GET";
            var resp = new StreamReader(req.GetResponse().GetResponseStream()).ReadToEnd();
            Taches = JsonConvert.DeserializeObject<List<Tache>>(resp);
            Device.BeginInvokeOnMainThread(() =>
            {
                TasksListView.ItemsSource = new ObservableCollection<String>(Taches.Select(x => x.ID + "-" + x.TechnicianID + "-" + x.ChamberID));
            });
        }

        private void TasksListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (TasksListView.SelectedItem != null)
            {
                Tache = Taches.FirstOrDefault(x => x.ID == int.Parse(TasksListView.SelectedItem.ToString().Split('-')[0]));
                TaskDetailsLabel.Text = $"ID: {Tache.ID}\n\n\n\nTechnician ID: {Tache.TechnicianID}\n\n\n\nChamberID: {Tache.ChamberID}\n\n\n\nTaskFinished: {Tache.TaskFinished} \n\n\n\nTask Description: {Tache.TaskDescription}";
            }
        }
    }
}