using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class PatientController : Controller
    {
        private readonly DatabaseHelper _db;
        private readonly Logger _logger;

        public PatientController(DatabaseHelper db, Logger logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: /Patient
        public IActionResult Index(string search = "")
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var patients = new List<Patient>();
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var query = string.IsNullOrEmpty(search)
                ? "SELECT * FROM Patients ORDER BY PatientID ASC"
: $"SELECT * FROM Patients WHERE FirstName LIKE '%{search}%' OR LastName LIKE '%{search}%' OR ContactNumber LIKE '%{search}%' ORDER BY PatientID ASC";
                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    patients.Add(new Patient
                    {
                        PatientID = reader.GetInt32("PatientID"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        DateOfBirth = reader.GetDateTime("DateOfBirth"),
                        Gender = reader.GetString("Gender"),
                        ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? "" : reader.GetString("ContactNumber"),
                        BloodGroup = reader.IsDBNull(reader.GetOrdinal("BloodGroup")) ? "" : reader.GetString("BloodGroup"),
                        Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "" : reader.GetString("Address")
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
            }

            ViewBag.Search = search;
            return View(patients);
        }

        // GET: /Patient/AddEdit
        public IActionResult AddEdit(int? id)
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var patient = new Patient();
            if (id.HasValue)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM Patients WHERE PatientID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        patient.PatientID = reader.GetInt32("PatientID");
                        patient.FirstName = reader.GetString("FirstName");
                        patient.LastName = reader.GetString("LastName");
                        patient.DateOfBirth = reader.GetDateTime("DateOfBirth");
                        patient.Gender = reader.GetString("Gender");
                        patient.ContactNumber = reader.IsDBNull(reader.GetOrdinal("ContactNumber")) ? "" : reader.GetString("ContactNumber");
                        patient.BloodGroup = reader.IsDBNull(reader.GetOrdinal("BloodGroup")) ? "" : reader.GetString("BloodGroup");
                        patient.Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "" : reader.GetString("Address");
                        patient.EmergencyContact = reader.IsDBNull(reader.GetOrdinal("EmergencyContact")) ? "" : reader.GetString("EmergencyContact");
                    }
                }
                catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }
            }
            return View(patient);
        }

        // POST: /Patient/AddEdit
        [HttpPost]
        public IActionResult AddEdit(Patient p)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                MySqlCommand cmd;
                if (p.PatientID == 0)
                {
                    cmd = new MySqlCommand(@"INSERT INTO Patients 
                        (FirstName,LastName,DateOfBirth,Gender,ContactNumber,Address,BloodGroup,EmergencyContact)
                        VALUES (@fn,@ln,@dob,@g,@cn,@addr,@bg,@ec)", conn);
                }
                else
                {
                    cmd = new MySqlCommand(@"UPDATE Patients SET 
                        FirstName=@fn, LastName=@ln, DateOfBirth=@dob, Gender=@g,
                        ContactNumber=@cn, Address=@addr, BloodGroup=@bg, EmergencyContact=@ec
                        WHERE PatientID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", p.PatientID);
                }
                cmd.Parameters.AddWithValue("@fn", p.FirstName);
                cmd.Parameters.AddWithValue("@ln", p.LastName);
                cmd.Parameters.AddWithValue("@dob", p.DateOfBirth);
                cmd.Parameters.AddWithValue("@g", p.Gender);
                cmd.Parameters.AddWithValue("@cn", p.ContactNumber);
                cmd.Parameters.AddWithValue("@addr", p.Address);
                cmd.Parameters.AddWithValue("@bg", p.BloodGroup);
                cmd.Parameters.AddWithValue("@ec", p.EmergencyContact);
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                ViewBag.Error = "Error saving patient: " + ex.Message;
                return View(p);
            }
        }

        // GET: /Patient/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Patients WHERE PatientID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }
            return RedirectToAction("Index");
        }
    }
}