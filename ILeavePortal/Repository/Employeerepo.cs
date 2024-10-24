
using ILeavePortal.Controllers;
using ILeavePortal.Models;
using ILeavePortal.Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace ILeavePortal.Repository
{
    public class Employeerepo
    {
        string cs = "Data Source=DESKTOP-6CAGAKO\\SQLEXPRESS;Initial Catalog=ILeavePortal;User ID=jinalp;Password=jinal12;Trust Server Certificate=True";
        private static string connectionString = ConnectionString.Get("Connection");


        public static DataTable GetEmployee()
        {
            DataTable dt = new();
            try
            {
                string query = "select * from Employee";
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    con.Open();
                    sqlCommand.CommandType = CommandType.Text;
                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    sqlCommand.Connection = con;
                    da.SelectCommand = sqlCommand;
                    con.Close();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return dt;
        }

        public static bool Employee(string[] employeelist)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    //SqlCommand cmd = new SqlCommand("INSERT INTO Venue (VenueName, VenueCost,VenueFilename,VenueFilePath)\r\nVALUES (@VenueName, @VenueCost,@VenueFilename,@VenueFilePath);", con);
                    SqlCommand cmd = new SqlCommand("CreateEmployee", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = employeelist[1];
                    cmd.Parameters.Add("@UserEmailId", SqlDbType.VarChar).Value = employeelist[2];
                    cmd.Parameters.Add("@Password", SqlDbType.Date).Value =employeelist[3];
                    cmd.Parameters.Add("@ConfirmPassword", SqlDbType.VarChar).Value = employeelist[3];
                    cmd.Parameters.Add("@Role", SqlDbType.Int).Value = Convert.ToInt32( employeelist[4]);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool EditEmployee(string[] employeelist)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("applyLeaveUpdateApplyLeaveDetail", con);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = employeelist[1];
                    cmd.Parameters.Add("@UserEmailId", SqlDbType.Date).Value = employeelist[2];
                    cmd.Parameters.Add("@Password", SqlDbType.Date).Value = employeelist[3];
                    cmd.Parameters.Add("@ConfirmPassword", SqlDbType.VarChar).Value = employeelist[3];
                    cmd.Parameters.Add("@Role", SqlDbType.Int).Value = Convert.ToInt32(employeelist[4]);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static DataTable FetchLeaveUsingId(string Id)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Leave where Id=@Id", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("Id", SqlDbType.Int).Value = Convert.ToInt32(Id);
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }



        public static bool AddEmployee(Employee employeeRequest)
        {
            try
            {
                string query = "INSERT INTO Employee (Name, UserEmailId, Password,ConfirmPassword, Role) VALUES (@Name, @UserEmailId, @Password, @ConfirmPassword, @Role)";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@Name", employeeRequest.Name);
                    sqlCommand.Parameters.AddWithValue("@UserEmailId", employeeRequest.UserEmailId);  // Convert DateOnly to DateTime
                    sqlCommand.Parameters.AddWithValue("@Password", employeeRequest.Password);      // Convert DateOnly to DateTime
                    sqlCommand.Parameters.AddWithValue("@ConfirmPassword", employeeRequest.Confirmpassword);
                    sqlCommand.Parameters.AddWithValue("@Role", employeeRequest.Role);

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

        public static bool Update(Employee updatedEmployee)
        {
            try
            {
                string query = "UPDATE Employee SET Name = @Name, UserEmailId = @USerEmailId, Password = @Password, ConfirmPassword = @ConfirmPassword, Role = @Role WHERE Id = @Id";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@Id", updatedEmployee.Id);
                    sqlCommand.Parameters.AddWithValue("@Name", updatedEmployee.Name);
                    sqlCommand.Parameters.AddWithValue("@UserEmailId", updatedEmployee.UserEmailId);
                    sqlCommand.Parameters.AddWithValue("@Password", updatedEmployee.Password);
                    sqlCommand.Parameters.AddWithValue("@Cofirmpassword", updatedEmployee.Confirmpassword);
                    sqlCommand.Parameters.AddWithValue("@Role", updatedEmployee.Role);

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

        public bool CheckEmailExists(string email,string password)
        {
            try
            {
                string query = "SELECT COUNT(1) FROM Employee WHERE UserEmailId = @UserEmailId AND Password = @Password";

                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    sqlCommand.Parameters.AddWithValue("@UserEmailId", email);
                    sqlCommand.Parameters.AddWithValue("@Password", password);
                    con.Open();
                    // Execute the command and check if any record exists
                    int count = (int)sqlCommand.ExecuteScalar(); // ExecuteScalar returns the first column of the first row
                    con.Close();

                    return count > 0; // If count is greater than 0, the email exists
                }
            }
            catch (Exception ex)
            {
                // Log the error (consider using a logging framework)
                Console.WriteLine($"Error checking email existence: {ex.Message}");
                return false; // Handle the exception gracefully
            }
        }


        //internal bool CancelEmployee(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //internal bool UpdateEmployee(Employee updatedemployee)
        //{
        //    throw new NotImplementedException();
        //}
    }
}





