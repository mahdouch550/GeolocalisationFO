using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GeolocalisationFO_Admin
{
    public class MyCell : ViewCell
    {
        public MyCell() : base()
        {
            RelativeLayout layout = new RelativeLayout();
            layout.SetBinding(Layout.BackgroundColorProperty, new Binding("BackgroundColor"));

            View = layout;
        }
    }
}