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

        public Admin()
        {

        }

        public void AjouterTechnicien(Technicien technicien)
        {

        }

        public void SupprimerTechnicien(Technicien technicien)
        {

        }

        public void ModifierTechnicien(Technicien oldTechnicien, Technicien newTechnicien)
        {

        }

        public void AjouterChambre(Technicien chambre)
        {

        }

        public void SupprimerChambre(Technicien chambre)
        {

        }

        public void ModifierChambre(Technicien oldCchambre, Technicien newChambre)
        {

        }
    }
}