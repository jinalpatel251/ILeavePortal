using ILeavePortal.Models;
using ILeavePortal.Utility;
using Microsoft.Data.SqlClient;
using System.Data;
using SqlConnection = Microsoft.Data.SqlClient.SqlConnection;

namespace ILeavePortal.Repository               
{
    public class Loginrepo
    {
        string cs = "Data Source=DESKTOP-6CAGAKO\\SQLEXPRESS;Initial Catalog=ILeavePortal;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";

        private static string connectionString = ConnectionString.Get("Connection");

        private static List<Employee> employees = new List<Employee>();

        public List<Employee> ListAll()
        {
            List<Employee> lst = new List<Employee>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetEmployee", con);
                com.CommandType = CommandType.StoredProcedure;
                SqlDataReader rdr = com.ExecuteReader();
                while (rdr.Read())
                {
                    lst.Add(new Employee
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        Name = rdr["Name"].ToString(),
                        UserEmailId =rdr["UserEmailId"].ToString(),
                        Password = rdr["Password"].ToString(),
                        Confirmpassword = rdr["Confirmpassword"].ToString(),
                        Role = Convert.ToInt32(rdr["Role"]),
                    });
                }
                return lst;

            }

        }

        public int Add(Login login)
        {
            int i = 0;

            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("GetLogin", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", login.Id);
                com.Parameters.AddWithValue("@UserEmailId", login.UserEmailId);
                com.Parameters.AddWithValue("@Password", login.Password);

                // Assuming your stored procedure returns a single value like a loginId
                int loginId = Convert.ToInt32(com.ExecuteScalar());

                // If loginId is returned, you can use it here
                if (loginId > 0)
                {
                    i = 1;  // Success
                }
            }

            return i;
        }



        public int AddEmployee(Employee employee)       
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {                  
                con.Open();
                SqlCommand com = new SqlCommand("CreateEmployee", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", employee.Id);
                com.Parameters.AddWithValue("@Name", employee.Name);
                com.Parameters.AddWithValue("@UserEmailId", employee.UserEmailId);
                com.Parameters.AddWithValue("@Password", employee.Password);
                com.Parameters.AddWithValue("@ConfirmPassword", employee.Confirmpassword);
                com.Parameters.AddWithValue("@Role", 1);
                //com.Parameters.AddWithValue("@Action", "Insert");
                i = com.ExecuteNonQuery();
            }
            return i;
        }

        //Method for Updating Employee record
        public int UpdateEmployee(Employee employee)    
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("UpdateEmployeeDetail", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", employee.Id);
                com.Parameters.AddWithValue("@Name", employee.Name);
                com.Parameters.AddWithValue("@UserEmailId", employee.UserEmailId);
                com.Parameters.AddWithValue("@Password", employee.Password);
                com.Parameters.AddWithValue("@Confirmpassword", employee.Confirmpassword);
                com.Parameters.AddWithValue("@Role", 1);
                //com.Parameters.AddWithValue("@Action", "Update");
                i = com.ExecuteNonQuery();
            }
            return i;
        }
        public static DataTable FetchEmployeeUsingId(string Id)
        {
            DataTable dt = new();
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {

                    SqlCommand cmd = new SqlCommand("select * from Employee where Id=@Id", con);
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


        //Method for Deleting an Employee
        public int Delete(int Id)
        {
            int i;
            using (SqlConnection con = new SqlConnection(cs))
            {
                con.Open();
                SqlCommand com = new SqlCommand("DeletebyEmployeeId", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Id", Id);
                i = com.ExecuteNonQuery();
            }
            return i;
        }

      

    }
}









