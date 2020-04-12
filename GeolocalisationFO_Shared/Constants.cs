using System;

namespace GeolocalisationFO_Shared
{
    public class Constants
    {
        private static String Port = "52640";
        private static String IPAddress = "192.168.43.175";
        public static readonly String APIURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/";

        //GET Methods URLs
        public static readonly String GetChambersURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/GetChambers";
        public static readonly String GetTechniciansURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/GetTechnicians";
        public static readonly String GetAllTasksURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/GetAllTasks";
        public static readonly String GetTechnicianURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/GetTechnician";
        public static readonly String GetChamberURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/GetChamber";
        public static readonly String GetMyTasksURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/GetMyTasks";

        //POST Methods URLs
        public static readonly String VerifyAdminLoginURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/VerifyAdminLogin";
        public static readonly String AddTechnicianURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/AddTechnician";
        public static readonly String AddChamberURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/AddChamber";
        public static readonly String AddTaskURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/AddTask";

        //DELETE Methods URLs
        public static readonly String DeleteTechnicianURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/DeleteTechnician";
        public static readonly String DeleteChamberURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/DeleteChamber";

        //PUT Methods URLs
        public static readonly String UpdateTechnicianURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/UpdateTechnician";
        public static readonly String UpdateChamberURL = "http://" + IPAddress + ":" + Port + "/api/GeolocalisationFO/UpdateChamber";
    }
}