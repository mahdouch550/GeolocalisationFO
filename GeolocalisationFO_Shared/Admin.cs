using System;
using System.Collections.Generic;

namespace GeolocalisationFO_Shared
{
    public class Admin
    {
        public String Login { get; set; }
        public String MotDePasse { get; set; }
        public List<Technicien> Techniciens { get; set; }
        public List<Technicien> Chambres { get; set; }
    }
}