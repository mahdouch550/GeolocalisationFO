using System;
using Xamarin.Forms;

namespace GeolocalisationFO_Admin
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            try
            {
                MainPage = new NavigationPage(new MainPage2());
            }
            catch(Exception ew)
            {
                Console.WriteLine(ew.Message);
                while(ew.InnerException != null)
                {
                    ew = ew.InnerException;
                    Console.WriteLine(ew.Message);
                }
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}