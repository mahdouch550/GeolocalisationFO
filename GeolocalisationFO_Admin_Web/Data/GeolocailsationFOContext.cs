using GeolocalisationFO_Shared;
using Microsoft.EntityFrameworkCore;

namespace GeolocalisationFO_Admin_Web.Data
{
    public class GeolocailsationFOContext : DbContext
    {
        public GeolocailsationFOContext(DbContextOptions<GeolocailsationFOContext> options) : base (options)
        {

        }

        public DbSet<Chambre>  Chambres { get; set; }
        public DbSet<Technicien> Techniciens { get; set; }
        public DbSet<Tache> Taches { get; set; }
        public DbSet<Admin> Admins { get; set; }
    }
}