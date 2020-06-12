using GeolocalisationFO_Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeolocalisationFO_Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage2 : MasterDetailPage
    {
        public MainPage2()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = ((MainPage2MasterMenuItem)e.SelectedItem).Title;
                if (item == null)
                    return;
                Page page;
                switch (item)
                {
                    case "Chambres":
                        {
                            page = new AllChambers() as AllChambers;
                            break;
                        }
                    case "Tâches":
                        {
                            page = new AllTasks() as AllTasks;
                            break;
                        }
                    case "Techniciens":
                        {
                            page = new AllTechnicians() as AllTechnicians;
                            break;
                        }
                    default: return;
                }
                page.Title = item;

                Detail = new NavigationPage(page);
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            catch
            {
                return;
            }
        }
    }
}