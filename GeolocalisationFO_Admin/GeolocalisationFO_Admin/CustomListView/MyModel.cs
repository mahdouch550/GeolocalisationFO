using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace GeolocalisationFO_Admin
{
    public class MyModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Color _backgroundColor;

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BackgroundColor"));
                }
            }
        }

        public void SetColors(bool isSelected)
        {
            if (isSelected)
            {
                BackgroundColor = Color.FromRgb(0.20, 0.20, 1.0);
            }
            else
            {
                BackgroundColor = Color.FromRgb(0.95, 0.95, 0.95);
            }
        }
    }
}