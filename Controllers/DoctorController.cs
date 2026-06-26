using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class DoctorController : Controller
    {
        private readonly DatabaseHelper _db;
        private readonly Logger _logger;

        public DoctorController(DatabaseHelper db, Logger logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index(string search = "")
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var doctors = new List<Doctor>();
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var query = string.IsNullOrEmpty(search)
                    ? "SELECT d.*, dep.DeptName FROM Doctors d LEFT JOIN Departments dep ON d.DeptID=dep.DeptID ORDER BY d.DoctorID ASC"
                    : $"SELECT d.*, dep.DeptName FROM Doctors d LEFT JOIN Departments dep ON d.DeptID=dep.DeptID WHERE d.FirstName LIKE '%{search}%' OR d.LastName LIKE '%{search}%' OR d.Specialization LIKE '%{search}%' ORDER BY d.DoctorID ASC";

                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorID = reader.GetInt32("DoctorID"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName"),
                        Specialization = reader.GetString("Specialization"),
                        DeptID = reader.IsDBNull(reader.GetOrdinal("DeptID")) ? 0 : reader.GetInt32("DeptID"),
                        DepartmentName = reader.IsDBNull(reader.GetOrdinal("DeptName")) ? "" : reader.GetString("DeptName"),
                        ContactNumber = reader.GetString("ContactNumber"),
                        Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "" : reader.GetString("Email"),
                        Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal("Salary")
                    });
                }
            }
            catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }

            ViewBag.Search = search;
            return View(doctors);
        }

        public IActionResult AddEdit(int? id)
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            LoadDepartments();
            var doctor = new Doctor();
            if (id.HasValue)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM Doctors WHERE DoctorID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        doctor.DoctorID = reader.GetInt32("DoctorID");
                        doctor.FirstName = reader.GetString("FirstName");
                        doctor.LastName = reader.GetString("LastName");
                        doctor.Specialization = reader.GetString("Specialization");
                        doctor.DeptID = reader.IsDBNull(reader.GetOrdinal("DeptID")) ? 0 : reader.GetInt32("DeptID");
                        doctor.ContactNumber = reader.GetString("ContactNumber");
                        doctor.Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? "" : reader.GetString("Email");
                        doctor.Salary = reader.IsDBNull(reader.GetOrdinal("Salary")) ? 0 : reader.GetDecimal("Salary");
                    }
                }
                catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }
            }
            return View(doctor);
        }

        [HttpPost]
        public IActionResult AddEdit(Doctor d)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                MySqlCommand cmd;
                if (d.DoctorID == 0)
                    cmd = new MySqlCommand("INSERT INTO Doctors (FirstName,LastName,Specialization,DeptID,ContactNumber,Email,Salary) VALUES (@fn,@ln,@sp,@dept,@cn,@em,@sal)", conn);
                else
                {
                    cmd = new MySqlCommand("UPDATE Doctors SET FirstName=@fn,LastName=@ln,Specialization=@sp,DeptID=@dept,ContactNumber=@cn,Email=@em,Salary=@sal WHERE DoctorID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", d.DoctorID);
                }
                cmd.Parameters.AddWithValue("@fn", d.FirstName);
                cmd.Parameters.AddWithValue("@ln", d.LastName);
                cmd.Parameters.AddWithValue("@sp", d.Specialization);
                cmd.Parameters.AddWithValue("@dept", d.DeptID);
                cmd.Parameters.AddWithValue("@cn", d.ContactNumber);
                cmd.Parameters.AddWithValue("@em", d.Email);
                cmd.Parameters.AddWithValue("@sal", d.Salary);
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                ViewBag.Error = "Error: " + ex.Message;
                LoadDepartments();
                return View(d);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Doctors WHERE DoctorID=@id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }
            return RedirectToAction("Index");
        }

        private void LoadDepartments()
        {
            var depts = new List<Department>();
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM Departments", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    depts.Add(new Department { DeptID = reader.GetInt32("DeptID"), DeptName = reader.GetString("DeptName") });
            }
            catch { }
            ViewBag.Departments = depts;
        }
    }
}