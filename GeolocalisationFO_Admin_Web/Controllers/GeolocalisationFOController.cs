using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using GeolocalisationFO_Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace GeolocalisationFO_Admin_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocalisationFOController : Controller
    {
        private String ConnectionString;

        public GeolocalisationFOController()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["GeoLocalisationFO_ConnectionString"].ConnectionString;
        }

        [HttpGet (Name="GetChambers")]
        [Route("GetChambers")]
        public List<Chambre> GetChambers()
        {
            var output = new List<Chambre>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select * from Chambres"; 
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        output.Add(new Chambre { Nom = reader.GetString(0), Longitude = float.Parse(reader.GetValue(1).ToString()), Latitude = float.Parse(reader.GetValue(2).ToString()) });
                    }
                    return output;
                }
            }
        }

        [HttpGet(Name = "GetTechnicians")]
        [Route ("GetTechnicians")]
        public List<Technicien> GetTechnicians()
        {
            var output = new List<Technicien>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = "select * from techniciens"; 
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        output.Add(new Technicien { Nom = reader.GetString(0), Login = reader.GetValue(1).ToString(), MotDePasse = reader.GetValue(2).ToString() });
                    }
                    return output;
                }
            }
        }

        [HttpPost(Name = "AddTechnician")]
        [Route("AddTechnician")]
        public ActionResult AddTechnician([FromBody] JObject jsonTechnicien)
        {
            var technicien = jsonTechnicien.ToObject<Technicien>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_add_technician";
                cmd.Parameters.AddWithValue("@nom", technicien.Nom);
                cmd.Parameters.AddWithValue("@login", technicien.Login);
                cmd.Parameters.AddWithValue("@motdepasse", technicien.MotDePasse);
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    var result = reader.GetValue(0).ToString();
                    if (result.Equals("true"))
                        return new OkObjectResult("Technician Added Successfully");
                    else
                        return new BadRequestObjectResult("Technician not added");
                }
            }
        }

        [HttpDelete(Name = "DeleteTechnician")]
        [Route("DeleteTechnician")]
        public ActionResult DeleteTechnician([FromBody] JObject jsonTechnicien)
        {
            var technicien = jsonTechnicien.ToObject<Technicien>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_delete_technician";
                cmd.Parameters.AddWithValue("@nom", technicien.Nom);
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    var result = reader.GetValue(0).ToString();
                    if (result.Equals("true"))
                        return new OkObjectResult("Technician deleted Successfully");
                    else
                        return new BadRequestObjectResult("Technician not deleted");
                }
            }
        }

        [HttpPut (Name = "UpdateTechnician")]
        [Route("UpdateTechnician")]
        public ActionResult UpdateTechnician([FromBody] JArray jsonOldTechnicien)
        {
            var OldTechnicien = jsonOldTechnicien.ToObject<List<Technicien>>().First();
            var NewTechnicien = jsonOldTechnicien.ToObject<List<Technicien>>().Last();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_update_technician";
                cmd.Parameters.AddWithValue("@oldnom", OldTechnicien.Nom);
                cmd.Parameters.AddWithValue("@login", NewTechnicien.Login);
                cmd.Parameters.AddWithValue("@motdepasse", NewTechnicien.MotDePasse);
                cmd.Parameters.AddWithValue("@newnom", NewTechnicien.Nom);
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    var result = reader.GetValue(0).ToString();
                    if (result.Equals("true"))
                        return new OkObjectResult("Technician updated Successfully");
                    else
                        return new BadRequestObjectResult("Technician not updated");
                }
            }
        }

        [HttpPost (Name = "AddChamber")]
        [Route("AddChamber")]
        public ActionResult AddChamber([FromBody] JObject jsonChamber)
        {
            var chambre = jsonChamber.ToObject<Chambre>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_add_chambre";
                cmd.Parameters.AddWithValue("@nom", chambre.Nom);
                cmd.Parameters.AddWithValue("@longitude", chambre.Longitude);
                cmd.Parameters.AddWithValue("@latitude", chambre.Latitude);
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = default(SqlDataReader);
                    try
                    {
                        reader = cmd.ExecuteReader();
                    }
                    catch (SqlException ex)
                    {
                        return new OkObjectResult("Chamber Name exists in the database");
                    }
                    reader.Read();
                    var result = reader.GetValue(0).ToString();
                    if (result.Equals("true"))
                        return new OkObjectResult("Chamber Added Successfully");
                    else
                        return new BadRequestObjectResult("Chamber not added");
                }
            }
        }

        [HttpPost(Name = "AddTask")]
        [Route("AddTask")]
        public ActionResult AddTask([FromBody] JObject jsonTask)
        {
            var task = jsonTask.ToObject<Tache>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_add_task";
                cmd.Parameters.AddWithValue("@technicianid", task.Technicien.Nom);
                cmd.Parameters.AddWithValue("@chamberid", task.Chambre.Nom);
                cmd.Parameters.AddWithValue("@taskdescription", task.DescriptionTache);
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = default(SqlDataReader);                    
                    reader = cmd.ExecuteReader();                    
                    reader.Read();
                    var result = reader.GetValue(0).ToString();
                    if (result.Equals("true"))
                        return new OkObjectResult("Task Added Successfully");
                    else
                        return new BadRequestObjectResult("Task not added");
                }
            }
        }

        [HttpDelete(Name = "DeleteChamber")]
        [Route("DeleteChamber")]
        public ActionResult DeleteChamber([FromBody] JObject jsonChamber)
        {
            var chambre = jsonChamber.ToObject<Technicien>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_delete_chambre";
                cmd.Parameters.AddWithValue("@nom", chambre.Nom);
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    var result = reader.GetValue(0).ToString();
                    if (result.Equals("true"))
                        return new OkObjectResult("Chamber deleted Successfully");
                    else
                        return new BadRequestObjectResult("Chamber not deleted");
                }
            }
        }

        [HttpDelete(Name = "DeleteTask")]
        [Route("DeleteTask")]
        public ActionResult DeleteTask(String TaskID)
        {
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_delete_task";
                cmd.Parameters.AddWithValue("@taskid", TaskID);
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        var result = reader.GetValue(0).ToString();
                        if (result.Equals("true"))
                            return new OkObjectResult("Task deleted Successfully");
                        else
                            return new BadRequestObjectResult("Task not deleted");
                    }
                }
            }
            return null;
        }

        [HttpPut(Name = "UpdateChamber")]
        [Route("UpdateChamber")]
        public ActionResult UpdateChamber([FromBody] JArray jsonOldChamber)
        {
            var OldChamber = jsonOldChamber.ToObject<List<Chambre>>().First();
            var NewChamber = jsonOldChamber.ToObject<List<Chambre>>().Last();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "sp_update_chambre";
                cmd.Parameters.AddWithValue("@oldnom", OldChamber.Nom);
                cmd.Parameters.AddWithValue("@longitude", NewChamber.Longitude);
                cmd.Parameters.AddWithValue("@latitude", NewChamber.Latitude);
                cmd.Parameters.AddWithValue("@newnom", NewChamber.Nom);
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    var result = reader.GetValue(0).ToString();
                    if (result.Equals("true"))
                        return new OkObjectResult("Chamber updated Successfully");
                    else
                        return new BadRequestObjectResult("Chamber not updated");
                }
            }
        }

        [HttpPost (Name = "VerifyAdminLogin")]
        [Route ("VerifyAdminLogin")]
        public bool VerifyAdminLogin([FromBody] JObject jsonAdmin)
        {
            var admin = jsonAdmin.ToObject<Admin>();
            using (var cmd = new SqlCommand())
            {
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.CommandText = $"select motdepasse from admins where login = '{admin.Login}' COLLATE Latin1_General_CS_AS ";                
                using (var cn = new SqlConnection(ConnectionString))
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State != System.Data.ConnectionState.Open)
                        cmd.Connection.Open();
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    if (reader.HasRows)
                    {
                        var result = reader.GetValue(0).ToString();
                        if (String.IsNullOrEmpty(result) || !result.Equals(admin.MotDePasse))
                            return false;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private Technicien GetTechnicien(String TechnicianName)
        {
            using (var cn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                        cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"select nom, login, motdepasse from techniciens where nom = '{TechnicianName}'";
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    if(reader.HasRows)
                        return new Technicien { Nom = reader.GetString(0), Login = reader.GetString(1), MotDePasse = reader.GetString(2) };
                }
            }
            return null;
        }

        private Chambre GetChamber(String ChambreName)
        {
            using (var cn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                        cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"select nom, longitude, latitude from chamres where nom = {ChambreName}";
                    var reader = cmd.ExecuteReader();
                    reader.Read();
                    if(reader.HasRows)
                        return new Chambre { Nom = reader.GetString(0), Latitude = reader.GetFloat(1), Longitude = reader.GetFloat(2) };
                }
            }
            return null;
        }

        [HttpGet(Name = "GetMyTasks")]
        [Route("GetMyTasks")]
        public List<Tache> GetMyTasks(String TechnicianName)
        {
            using (var cn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                        cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"select * from taches where technicianid = '{TechnicianName}'";
                    var reader = cmd.ExecuteReader();
                    var output = new List<Tache>();
                    while (reader.Read())
                    {
                        var tmpTask = new Tache();
                        tmpTask.ID = reader.GetInt32(0);
                        tmpTask.Technicien = GetTechnicien(reader.GetString(1));
                        try
                        {
                            tmpTask.Chambre = GetChamber(reader.GetString(2));
                        }
                        catch
                        {}
                        tmpTask.DescriptionTache = reader.GetString(3);
                        tmpTask.TacheFinie = reader.GetBoolean(4);
                        output.Add(tmpTask);
                    }
                    return output;
                }
            }
        }

        [HttpGet(Name = "GetAllTasks")]
        [Route("GetAllTasks")]
        public List<Tache> GetAllTasks()
        {
            using (var cn = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = cn;
                    if (cmd.Connection.State == System.Data.ConnectionState.Closed)
                        cmd.Connection.Open();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $"select * from taches";
                    var reader = cmd.ExecuteReader();
                    var output = new List<Tache>();
                    while (reader.Read())
                    {
                        var tmpTask = new Tache();
                        tmpTask.ID = reader.GetInt32(0);
                        tmpTask.Technicien = GetTechnicien(reader.GetString(1));
                        try
                        {
                            tmpTask.Chambre = GetChamber(reader.GetString(2));
                        }
                        catch
                        { }
                        tmpTask.DescriptionTache = reader.GetString(3);
                        tmpTask.TacheFinie = reader.GetBoolean(4);
                        output.Add(tmpTask);
                    }
                    return output;
                }
            }
        }
    }
}