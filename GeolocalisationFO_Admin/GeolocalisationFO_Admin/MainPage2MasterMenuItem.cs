using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeolocalisationFO_Admin
{

    public class MainPage2MasterMenuItem
    {
        public MainPage2MasterMenuItem()
        {
            TargetType = typeof(MainPage2MasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}