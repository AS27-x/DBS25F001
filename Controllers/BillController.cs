using HospitalManagementSystem.Models;
using HospitalManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class BillController : Controller
    {
        private readonly DatabaseHelper _db;
        private readonly Logger _logger;

        public BillController(DatabaseHelper db, Logger logger)
        {
            _db = db;
            _logger = logger;
        }

        public IActionResult Index(string search = "")
        {
            if (HttpContext.Session.GetString("Username") == null)
                return RedirectToAction("Login", "Account");

            var list = new List<Bill>();
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var query = string.IsNullOrEmpty(search)
                    ? @"SELECT b.*, CONCAT(p.FirstName,' ',p.LastName) AS PatientName
                        FROM Bills b JOIN Patients p ON b.PatientID=p.PatientID
                        ORDER BY b.BillID ASC"
                    : $@"SELECT b.*, CONCAT(p.FirstName,' ',p.LastName) AS PatientName
                        FROM Bills b JOIN Patients p ON b.PatientID=p.PatientID
                        WHERE p.FirstName LIKE '%{search}%' OR p.LastName LIKE '%{search}%'
                        OR b.Status LIKE '%{search}%'
                        ORDER BY b.BillID ASC";

                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Bill
                    {
                        BillID = reader.GetInt32("BillID"),
                        PatientID = reader.GetInt32("PatientID"),
                        PatientName = reader.GetString("PatientName"),
                        AdmissionID = reader.IsDBNull(reader.GetOrdinal("AdmissionID")) ? 0 : reader.GetInt32("AdmissionID"),
                        TotalAmount = reader.GetDecimal("TotalAmount"),
                        PaidAmount = reader.GetDecimal("PaidAmount"),
                        BillDate = reader.GetDateTime("BillDate"),
                        Status = reader.GetString("Status")
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
            var bill = new Bill { BillDate = DateTime.Now };

            if (id.HasValue)
            {
                try
                {
                    using var conn = _db.GetConnection();
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM Bills WHERE BillID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        bill.BillID = reader.GetInt32("BillID");
                        bill.PatientID = reader.GetInt32("PatientID");
                        bill.AdmissionID = reader.IsDBNull(reader.GetOrdinal("AdmissionID")) ? 0 : reader.GetInt32("AdmissionID");
                        bill.TotalAmount = reader.GetDecimal("TotalAmount");
                        bill.PaidAmount = reader.GetDecimal("PaidAmount");
                        bill.BillDate = reader.GetDateTime("BillDate");
                        bill.Status = reader.GetString("Status");
                    }
                }
                catch (Exception ex) { _logger.LogError(ex.Message, ex.StackTrace); }
            }
            return View(bill);
        }

        [HttpPost]
        public IActionResult AddEdit(Bill b)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                MySqlCommand cmd;
                if (b.BillID == 0)
                    cmd = new MySqlCommand(@"INSERT INTO Bills (PatientID,TotalAmount,PaidAmount,BillDate,Status)
                        VALUES (@pid,@total,@paid,@date,@status)", conn);
                else
                {
                    cmd = new MySqlCommand(@"UPDATE Bills SET PatientID=@pid,TotalAmount=@total,
                        PaidAmount=@paid,BillDate=@date,Status=@status WHERE BillID=@id", conn);
                    cmd.Parameters.AddWithValue("@id", b.BillID);
                }
                cmd.Parameters.AddWithValue("@pid", b.PatientID);
                cmd.Parameters.AddWithValue("@total", b.TotalAmount);
                cmd.Parameters.AddWithValue("@paid", b.PaidAmount);
                cmd.Parameters.AddWithValue("@date", b.BillDate);
                cmd.Parameters.AddWithValue("@status", b.Status);
                cmd.ExecuteNonQuery();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                ViewBag.Error = "Error: " + ex.Message;
                LoadPatients();
                return View(b);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM Bills WHERE BillID=@id", conn);
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
    }
}