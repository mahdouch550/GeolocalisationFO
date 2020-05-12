using System;
using System.ComponentModel.DataAnnotations;

namespace GeolocalisationFO_Shared
{
    public class Tache
    {
        public String TechnicianLogin { get; set; }
        public String ChamberID { get; set; }
        public String TaskDescription { get; set; }
        [Key]
        public int ID { get; set; }
        public bool TaskFinished { get; set; }
    }
}