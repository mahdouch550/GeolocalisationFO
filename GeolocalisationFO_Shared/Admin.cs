using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GeolocalisationFO_Shared
{
    public class Admin
    {
        [Key]
        public String Login { get; set; }
        public String MotDePasse { get; set; }
    }
}