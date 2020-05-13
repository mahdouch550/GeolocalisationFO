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
using GeolocalisationFO_Admin_Web.Data;

namespace GeolocalisationFO_Admin_Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeolocalisationFOController : Controller
    {
        private GeolocailsationFOContext geolocalisationFOContext;

        public GeolocalisationFOController(GeolocailsationFOContext geolocalisationFOContext)
        {
            this.geolocalisationFOContext = geolocalisationFOContext;
        }

        #region GET Methods
        [HttpGet(Name = "GetChambers")]
        [Route("GetChambers")]
        public List<Chambre> GetChambers()
        {
            return geolocalisationFOContext.Chambres.ToList();
        }

        [HttpGet(Name = "GetTechnicians")]
        [Route("GetTechnicians")]
        public List<Technicien> GetTechnicians()
        {
            return geolocalisationFOContext.Techniciens.ToList();
        }

        [HttpGet(Name = "GetMyTasks")]
        [Route("GetMyTasks")]
        public List<Tache> GetMyTasks(String TechnicianLogin)
        {
            var taches = geolocalisationFOContext.Taches.ToList();
            return taches.Where(x => x.TechnicianID.Equals(TechnicianLogin)).ToList();
        }

        [HttpGet(Name = "GetAllTasks")]
        [Route("GetAllTasks")]
        public List<Tache> GetAllTasks()
        {
            return geolocalisationFOContext.Taches.ToList();
        }
        #endregion

        #region POST Methods
        [HttpPost(Name = "VerifyAdminLogin")]
        [Route("VerifyAdminLogin")]
        public bool VerifyAdminLogin([FromBody] JObject jsonAdmin)
        {
            var admin = jsonAdmin.ToObject<Admin>();
            return geolocalisationFOContext.Admins.FirstOrDefault(x => x.Login.Equals(admin.Login)) != null;
        }

        [HttpPost(Name = "VerifyTechnicianLogin")]
        [Route("VerifyTechnicianLogin")]
        public Technicien VerifyTechnicianLogin([FromBody] JObject jsonTechnicia)
        {
            var technicien = jsonTechnicia.ToObject<Technicien>();
            return geolocalisationFOContext.Techniciens.FirstOrDefault(x => x.Login.Equals(technicien.Login) && x.MotDePasse.Equals(technicien.MotDePasse));
        }

        [HttpPost(Name = "AddChamber")]
        [Route("AddChamber")]
        public ActionResult AddChamber([FromBody] JObject jsonChamber)
        {
            var chambre = jsonChamber.ToObject<Chambre>();
            try
            {
                geolocalisationFOContext.Chambres.Add(chambre);
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Chamber Added Successfully");
            }
            catch (Exception ex)
            {
                return new OkObjectResult("Chamber Name exists in the database");
            }
        }

        [HttpPost(Name = "AddTechnician")]
        [Route("AddTechnician")]
        public ActionResult AddTechnician([FromBody] JObject jsonTechnicien)
        {
            var technicien = jsonTechnicien.ToObject<Technicien>();
            try
            {
                geolocalisationFOContext.Techniciens.Add(technicien);
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Technician Added Successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Technician not added");
            }
        }

        [HttpPost(Name = "AddTask")]
        [Route("AddTask")]
        public ActionResult AddTask([FromBody] JObject jsonTask)
        {
            var task = jsonTask.ToObject<Tache>();
            try
            {
                geolocalisationFOContext.Taches.Add(task);
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Task Added Successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Task not added");
            }
        }
        #endregion

        #region DELETE Methods
        [HttpDelete(Name = "DeleteTechnician")]
        [Route("DeleteTechnician")]
        public ActionResult DeleteTechnician([FromBody] JObject jsonTechnicien)
        {
            var technicien = jsonTechnicien.ToObject<Technicien>();
            try
            {
                geolocalisationFOContext.Techniciens.Remove(technicien);
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Technician deleted Successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Technician not deleted");
            }
        }

        [HttpDelete(Name = "DeleteChamber")]
        [Route("DeleteChamber")]
        public ActionResult DeleteChamber([FromBody] JObject jsonChamber)
        {
            var chambre = jsonChamber.ToObject<Chambre>();
            try
            {
                geolocalisationFOContext.Chambres.Remove(chambre);
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Chamber deleted Successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Chamber not deleted");
            }
        }


        [HttpDelete(Name = "DeleteTask")]
        [Route("DeleteTask")]
        public ActionResult DeleteTask(String TaskID)
        {
            try
            {
                var task = geolocalisationFOContext.Taches.Find(new string[] { TaskID });
                geolocalisationFOContext.Taches.Remove(task);
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Task deleted Successfully");
            }
            catch(Exception ex)
            {
                return new BadRequestObjectResult("Task not deleted");
            }
                    
        }
        #endregion

        #region PUT Methods
        [HttpPut(Name = "UpdateTechnician")]
        [Route("UpdateTechnician")]
        public ActionResult UpdateTechnician([FromBody] JArray jsonOldTechnicien)
        {
            var OldTechnicien = jsonOldTechnicien.ToObject<List<Technicien>>().First();
            var NewTechnicien = jsonOldTechnicien.ToObject<List<Technicien>>().Last();
            try
            {
                geolocalisationFOContext.Techniciens.Remove(OldTechnicien);
                geolocalisationFOContext.Techniciens.Add(NewTechnicien);
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Technician updated Successfully");
            }
            catch (Exception ex) 
            {
                return new BadRequestObjectResult("Technician not updated");
            }
        }

        [HttpPut(Name = "UpdateChamber")]
        [Route("UpdateChamber")]
        public ActionResult UpdateChamber([FromBody] JArray jsonOldChamber)
        {
            var OldChamber = jsonOldChamber.ToObject<List<Chambre>>().First();
            var NewChamber = jsonOldChamber.ToObject<List<Chambre>>().Last();
            try
            {
                geolocalisationFOContext.Chambres.Remove(OldChamber);
                geolocalisationFOContext.Chambres.Add(NewChamber);
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Chamber updated Successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Chamber not updated");
            }
        }

        [HttpPut(Name = "UpdateTaskFinished")]
        [Route("UpdateTaskFinished")]
        public ActionResult UpdateTaskFinished([FromBody] JObject jsonOldTask)
        {
            var oldTask = jsonOldTask.ToObject<Tache>();            
            try
            {
                geolocalisationFOContext.Taches.FirstOrDefault(x => x.ID == oldTask.ID).TaskFinished = true;
                geolocalisationFOContext.SaveChanges();
                return new OkObjectResult("Task updated Successfully");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult("Task not updated");
            }
        }
        #endregion
    }
}