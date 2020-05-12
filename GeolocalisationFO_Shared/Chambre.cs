using System;
using System.ComponentModel.DataAnnotations;

namespace GeolocalisationFO_Shared
{
    public class Chambre
    {
        [Key]
        public String Nom { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}