using System;

namespace GeolocalisationFO_Shared
{
    public class Tache
    {
        public Technicien Technicien { get; set; }
        public Chambre Chambre { get; set; }
        public String DescriptionTache { get; set; }
        public int ID { get; set; }
        public bool TacheFinie { get; set; }
    }
}