using System;
using System.ComponentModel.DataAnnotations;

namespace GeolocalisationFO_Shared
{
	public class Technicien
	{
		public String Nom { get; set; }
		[Key]
		public String Login { get; set; }
		public String MotDePasse { get; set; }
	}
}