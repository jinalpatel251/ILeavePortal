using ILeavePortal.Models;
using ILeavePortal.Utility;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Web.WebPages;
using static System.Runtime.InteropServices.JavaScript.JSType;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace ILeavePortal.Repository
{
    public class ApplyLeaveRepo
    {
        string cs = "Data Source=DESKTOP-6CAGAKO\\SQLEXPRESS;Initial Catalog=ILeavePortal;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
        private static string connectionString = ConnectionString.Get("Connection");
        public static DataTable GetApplyLeave()
        {
            DataTable dt = new();
            try
            {
                string query = "select * from Leave";
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

        public static async Task<DataTable> GetLeaveByIdAsync(int id)
        {
            DataTable dt = new();
            try
            {
                string query = "SELECT * FROM Leave WHERE Id = @Id";
                using (SqlConnection con = new SqlConnection(connectionString))
                using (SqlCommand sqlCommand = new SqlCommand(query, con))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@Id", id);  // Add the Id parameter

                    SqlDataAdapter da = new SqlDataAdapter(sqlCommand);
                    await con.OpenAsync();  // Open connection asynchronously
                    da.Fill(dt);  // Fill the DataTable with the leave details (SqlDataAdapter does not support async methods)
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                Console.WriteLine(ex.Message);
            }
            return dt;
        }

        //public static bool ApplyLeave(string[] leavelist)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            //SqlCommand cmd = new SqlCommand("INSERT INTO Venue (VenueName, VenueCost,VenueFilename,VenueFilePath)\r\nVALUES (@VenueName, @VenueCost,@VenueFilename,@VenueFilePath);", con);
        //            SqlCommand cmd = new SqlCommand("CreateApplyLeave", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@LeaveType", SqlDbType.VarChar).Value = leavelist[1];
        //            cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = Convert.ToDateTime(leavelist[2]).Date;
        //            cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = Convert.ToDateTime(leavelist[3]).Date;
        //            cmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = Convert.ToString(leavelist[4]);
        //            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Pending";

        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}



        //public static bool EditLeave(string[] leavelist)
        //{
        //    try
        //    {
        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        {
        //            SqlCommand cmd = new SqlCommand("applyLeaveUpdateApplyLeaveDetail", con);

        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@LeaveType", SqlDbType.VarChar).Value = leavelist[1];
        //            cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = Convert.ToInt32(leavelist[2]);
        //            cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = Convert.ToDateTime(leavelist[3]).Date;
        //            cmd.Parameters.Add("@Reason", SqlDbType.VarChar).Value = Convert.ToDateTime(leavelist[3]).Date;
        //            cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = "Pending";

        //            con.Open();
        //            cmd.ExecuteNonQuery();
        //            con.Close();
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}


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

        public static async Task<bool> AddLeaveAsync(ApplyLeave applyLeave)
        {
            try
            {
                string procedure = "CreateApplyLeave";
                var parameters = new[]
                {
            new SqlParameter("@LeaveType", applyLeave.LeaveType),
            new SqlParameter("@StartDate", applyLeave.StartDate),
            new SqlParameter("@EndDate", applyLeave.EndDate),
            new SqlParameter("@Reason", applyLeave.Reason),
            new SqlParameter("@Status", "Pending")
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
        public static async Task<bool> UpdateLeaveAsync(ApplyLeave applyLeave)
        {
            try
            {
                string procedure = "UpdateApplyLeave";
                var parameters = new[]
                {
            new SqlParameter("@Id", applyLeave.Id),
            new SqlParameter("@LeaveType", applyLeave.LeaveType),
            new SqlParameter("@StartDate", applyLeave.StartDate),
            new SqlParameter("@EndDate", applyLeave.EndDate),
            new SqlParameter("@Reason", applyLeave.Reason),
            new SqlParameter("@Status", applyLeave.Status)
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

        public static bool CancelApplyLeave(int id)
        {
            try
            {
                string query = "DELETE FROM Leave WHERE Id = @Id";  // Or you could update the status instead of deleting

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






        //public static bool AddApplyLeave(ApplyLeave leaveRequest)
        //{
        //    try
        //    {
        //        string query = "INSERT INTO Leave (LeaveType, StartDate, EndDate, Reason, Status) VALUES (@LeaveType, @StartDate, @EndDate, @Reason, @Status)";

        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        using (SqlCommand sqlCommand = new SqlCommand(query, con))
        //        {
        //            sqlCommand.CommandType = CommandType.Text;
        //            sqlCommand.Parameters.AddWithValue("@LeaveType", leaveRequest.LeaveType);
        //            sqlCommand.Parameters.AddWithValue("@StartDate", leaveRequest.StartDate.ToDateTime(TimeOnly.MinValue));  // Convert DateOnly to DateTime
        //            sqlCommand.Parameters.AddWithValue("@EndDate", leaveRequest.EndDate.ToDateTime(TimeOnly.MinValue));      // Convert DateOnly to DateTime
        //            sqlCommand.Parameters.AddWithValue("@Reason", leaveRequest.Reason);
        //            sqlCommand.Parameters.AddWithValue("@Status", leaveRequest.Status);

        //            con.Open();
        //            int rowsAffected = sqlCommand.ExecuteNonQuery();
        //            con.Close();

        //            return rowsAffected > 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();  // Ideally, log the error
        //        return false;
        //    }
        //}

        //public static bool UpdateApplyLeave(ApplyLeave updatedLeave)
        //{
        //    try
        //    {
        //        string query = "UPDATE Leave SET LeaveType = @LeaveType, StartDate = @StartDate, EndDate = @EndDate, Reason = @Reason, Status = @Status WHERE Id = @Id";

        //        using (SqlConnection con = new SqlConnection(connectionString))
        //        using (SqlCommand sqlCommand = new SqlCommand(query, con))
        //        {
        //            sqlCommand.CommandType = CommandType.Text;
        //            sqlCommand.Parameters.AddWithValue("@Id", updatedLeave.Id);
        //            sqlCommand.Parameters.AddWithValue("@LeaveType", updatedLeave.LeaveType);
        //            sqlCommand.Parameters.AddWithValue("@StartDate", updatedLeave.StartDate.ToDateTime(TimeOnly.MinValue));
        //            sqlCommand.Parameters.AddWithValue("@EndDate", updatedLeave.EndDate.ToDateTime(TimeOnly.MinValue));
        //            sqlCommand.Parameters.AddWithValue("@Reason", updatedLeave.Reason);
        //            sqlCommand.Parameters.AddWithValue("@Status", updatedLeave.Status);

        //            con.Open();
        //            int rowsAffected = sqlCommand.ExecuteNonQuery();
        //            con.Close();

        //            return rowsAffected > 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();  // Ideally, log the error
        //        return false;
        //    }
        //}




    }
}









