using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly DatabaseHelper _db;
        private readonly Logger _logger;

        public AppointmentController(DatabaseHelper db, Logger logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index(string search = "")
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var list = new List<Appointment>();
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var query = string.IsNullOrEmpty(search)
                    ? @"SELECT a.*, CONCAT(p.FirstName,' ',p.LastName) AS PatientName,
                        CONCAT(d.FirstName,' ',d.LastName) AS DoctorName
                        FROM Appointments a
                        JOIN Patients p ON a.PatientID=p.PatientID
                        JOIN Doctors d ON a.DoctorID=d.DoctorID
                        ORDER BY a.AppointmentID ASC"
                    : $@"SELECT a.*, CONCAT(p.FirstName,' ',p.LastName) AS PatientName,
                        CONCAT(d.FirstName,' ',d.LastName) AS DoctorName
                        FROM Appointments a
                        JOIN Patients p ON a.PatientID=p.PatientID
                        JOIN Doctors d ON a.DoctorID=d.DoctorID
                        WHERE p.FirstName LIKE '%{search}%' OR p.LastName LIKE '%{search}%'
                        OR d.FirstName LIKE '%{search}%' OR a.Status LIKE '%{search}%'
                        ORDER BY a.AppointmentID ASC";

                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Appointment
                    {
                        AppointmentID = reader.GetInt32("AppointmentID"),
                        PatientID = reader.GetInt32("PatientID"),
                        PatientName = reader.GetString("PatientName"),
                        DoctorID = reader.GetInt32("DoctorID"),
                        DoctorName = reader.GetString("DoctorName"),
                        AppointmentDate = reader.GetDateTime("AppointmentDate"),
                        Status = reader.GetString("Status"),
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString("Notes")
                    });
                }
            }
            catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }

            ViewBag.Search = search;
            return View(list);
        }

        public IActionResult AddEdit(int? id)
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            LoadPatients();
            LoadDoctors();
            var appt = new Appointment { AppointmentDate = DateTime.Now };

            if (id.HasValue)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM Appointments WHERE AppointmentID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        appt.AppointmentID = reader.GetInt32("AppointmentID");
                        appt.PatientID = reader.GetInt32("PatientID");
                        appt.DoctorID = reader.GetInt32("DoctorID");
                        appt.AppointmentDate = reader.GetDateTime("AppointmentDate");
                        appt.Status = reader.GetString("Status");
                        appt.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? "" : reader.GetString("Notes");
                    }
                }
                catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }
            }
            return View(appt);
        }

        [HttpPost]
        public IActionResult AddEdit(Appointment a)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                MySqlCommand cmd;
                if (a.AppointmentID == 0)
                    cmd = new MySqlCommand(@"INSERT INTO Appointments
                        (PatientID,DoctorID,AppointmentDate,Status,Notes)
                        VALUES (@pid,@did,@date,@status,@notes)", conn);
                else
                {
                    cmd = new MySqlCommand(@"UPDATE Appointments SET
                        PatientID=@pid,DoctorID=@did,AppointmentDate=@date,
                        Status=@status,Notes=@notes WHERE AppointmentID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", a.AppointmentID);
                }
                cmd.Parameters.AddWithValue("@pid", a.PatientID);
                cmd.Parameters.AddWithValue("@did", a.DoctorID);
                cmd.Parameters.AddWithValue("@date", a.AppointmentDate);
                cmd.Parameters.AddWithValue("@status", a.Status);
                cmd.Parameters.AddWithValue("@notes", a.Notes);
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                ViewBag.Error = "Error: " + ex.Message;
                LoadPatients();
                LoadDoctors();
                return View(a);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Appointments WHERE AppointmentID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }
            return RedirectToAction("Index");
        }

        private void LoadPatients()
        {
            var list = new List<Patient>();
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand("SELECT PatientID, FirstName, LastName FROM Patients ORDER BY FirstName", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new Patient { PatientID = reader.GetInt32("PatientID"), FirstName = reader.GetString("FirstName"), LastName = reader.GetString("LastName") });
            }
            catch { }
            ViewBag.Patients = list;
        }

        private void LoadDoctors()
        {
            var list = new List<Doctor>();
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand("SELECT DoctorID, FirstName, LastName, Specialization FROM Doctors ORDER BY FirstName", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    list.Add(new Doctor { DoctorID = reader.GetInt32("DoctorID"), FirstName = reader.GetString("FirstName"), LastName = reader.GetString("LastName"), Specialization = reader.GetString("Specialization") });
            }
            catch { }
            ViewBag.Doctors = list;
        }
    }
}