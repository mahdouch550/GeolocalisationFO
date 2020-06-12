using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage2Master : ContentPage
    {
        public ListView ListView;

        public MainPage2Master()
        {
            InitializeComponent();

            BindingContext = new MainPage2MasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainPage2MasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainPage2MasterMenuItem> MenuItems { get; set; }

            public MainPage2MasterViewModel()
            {
                MenuItems = new ObservableCollection<MainPage2MasterMenuItem>(new[]
                {
                    new MainPage2MasterMenuItem { Id = 0, Title = "Chambres" },
                    new MainPage2MasterMenuItem { Id = 1, Title = "Techniciens" },
                    new MainPage2MasterMenuItem { Id = 2, Title = "Tâches" }
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}