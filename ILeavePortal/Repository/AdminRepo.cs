using ILeavePortal.Models;
using ILeavePortal.Utility;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using System.Web.WebPages;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace ILeavePortal.Repository
{
    public class AdminRepo
    {
        private static readonly string connectionString = ConnectionString.Get("Connection");

        private static async Task<DataTable> ExecuteQueryAsync(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new(connectionString))
                using (SqlCommand cmd = new(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    SqlDataAdapter da = new(cmd);
                    await con.OpenAsync();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                // Log exception using a logging framework
                // For example: _logger.LogError(ex, "Error executing query.");
                // Consider rethrowing or handling the exception as needed
            }
            return dt;
        }

        public static Task<DataTable> GetLeaveDetailsAsync()
        {
            string query = "SELECT * FROM Leave";
            return ExecuteQueryAsync(query);
        }

        public static Task<DataTable> GetLeaveDetailByIdAsync(int id)
        {
            string query = "SELECT * FROM Leave WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", id) };
            return ExecuteQueryAsync(query, parameters);
        }

        private static async Task<bool> UpdateLeaveStatusAsync(string[] details, string status)
        {
            try
            {
                string query = "UPDATE Leave SET LeaveType=@LeaveType, StartDate=@StartDate, EndDate=@EndDate, Reason=@Reason, Status=@Status WHERE Id = @Id";

      
                var parameters = new[]
                {
            new SqlParameter("@Id", SqlDbType.Int) { Value = Convert.ToInt32(details[0]) },
            new SqlParameter("@LeaveType", SqlDbType.VarChar) { Value = details[1] },
            new SqlParameter("@StartDate", SqlDbType.Date) { Value = details[2] },
            new SqlParameter("@EndDate", SqlDbType.Date) { Value = details[3] },
            new SqlParameter("@Reason", SqlDbType.VarChar) { Value = details[4] },
            new SqlParameter("@Status", SqlDbType.VarChar) { Value = status }
        };

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Log specific errors for invalid dates
                Console.WriteLine(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static async Task<bool> ApproveAsync(string[] details)
        {
            return await UpdateLeaveStatusAsync(details, "Approved");
        }

        public static async Task<bool> RejectAsync(string[] details)
        {
            return await UpdateLeaveStatusAsync(details, "Rejected");
        }

        public static Task<DataTable> GetEmployeeAsync()
        {
            string query = "SELECT * FROM Employee";
            return ExecuteQueryAsync(query);
        }

        public static Task<DataTable> GetEmployeeByIdAsync(int id)
        {
            string query = "SELECT * FROM Employee WHERE Id = @Id";
            var parameters = new[] { new SqlParameter("@Id", id) };
            return ExecuteQueryAsync(query, parameters);
        }



        public static async Task<bool> AddEmployeeAsync(Employee employee)
        {
            try
            {
                string procedure = "CreateEmployee";
                var parameters = new[]
                {
            new SqlParameter("@Name", employee.Name),
            new SqlParameter("@UserEmailId", employee.UserEmailId),
            new SqlParameter("@Password", employee.Password),
            new SqlParameter("@ConfirmPassword", employee.Confirmpassword),
            new SqlParameter("@Role", 1)
        };

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(procedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log exception here
                return false;
            }
        }
        public static async Task<bool> UpdateEmployeeAsync(Employee employee)
        {
            try
            {
                string procedure = "UpdateEmployeeDetail";
                var parameters = new[]
                {
            new SqlParameter("@Id", employee.Id),
            new SqlParameter("@Name", employee.Name),
            new SqlParameter("@UserEmailId", employee.UserEmailId),
            new SqlParameter("@Password", employee.Password),
            new SqlParameter("@ConfirmPassword", employee.Confirmpassword),
            new SqlParameter("@Role", employee.Role)
        };

                using (SqlConnection con = new(connectionString))
                using (SqlCommand cmd = new(procedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);

                    await con.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Log exception
                return false;
            }
        }

        public static bool CancelEmployee(int id)
        {
            try
            {
                string query = "DELETE FROM Employee WHERE Id = @Id";  // Or you could update the status instead of deleting

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@Id", id);

                    con.Open();
                    int rowsAffected = sqlCommand.ExecuteNonQuery();
                    con.Close();

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();  // Ideally, log the error
                return false;
            }
        }

    }
}
