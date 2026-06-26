using HospitalManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace HospitalManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly DatabaseHelper _db;
        private readonly Logger _logger;

        public AccountController(DatabaseHelper db, Logger logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
                return RedirectToAction("Index", "Dashboard");
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand(
                    "SELECT UserID, Username, Role FROM Users WHERE Username=@u AND PasswordHash=@p AND IsActive=1", conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password); // plain text for now
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    HttpContext.Session.SetInt32("UserID", reader.GetInt32("UserID"));
                    HttpContext.Session.SetString("Username", reader.GetString("Username"));
                    HttpContext.Session.SetString("Role", reader.GetString("Role"));
                    return RedirectToAction("Index", "Dashboard");
                }

                ViewBag.Error = "Invalid username or password.";
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex.StackTrace);
                ViewBag.Error = "A system error occurred. Please try again.";
                return View();
            }
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}