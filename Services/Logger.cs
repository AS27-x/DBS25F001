using MySql.Data.MySqlClient;

namespace HospitalManagementSystem.Services
{
    public class Logger
    {
        private readonly DatabaseHelper _db;

        public Logger(DatabaseHelper db)
        {
            _db = db;
        }

        public void LogError(string message, string stackTrace, string loggedBy = "System")
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO ErrorLogs(ErrorMessage, StackTrace, LoggedBy) VALUES (@msg, @stack, @user)", conn);
                cmd.Parameters.AddWithValue("@msg", message);
                cmd.Parameters.AddWithValue("@stack", stackTrace ?? "");
                cmd.Parameters.AddWithValue("@user", loggedBy);
                cmd.ExecuteNonQuery();
            }
            catch { /* silent fail to avoid infinite loop */ }
        }
    }
}