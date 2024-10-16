using ILeavePortal.Models;
using ILeavePortal.Repository;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace ILeavePortal.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LeaveApproved(string data)
        {
            try
            {
                string[]? status = JsonConvert.DeserializeObject<string[]>(data);

                var isApproved = await AdminRepo.ApproveAsync(status);

                if (isApproved)
                {
                    return Json(new { IsSuccess = true, Message = "Approved" });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "An error occurred" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error in LeaveApproved");
                return Json(new { IsSuccess = false, Message = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult> LeaveRejected(string data)
        {
            try
            {
                string[]? status = JsonConvert.DeserializeObject<string[]>(data);

                var isRejected = await AdminRepo.RejectAsync(status);

                if (isRejected)
                {
                    return Json(new { IsSuccess = true, Message = "Rejected" });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "An error occurred" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error in LeaveRejected");
                return Json(new { IsSuccess = false, Message = "An unexpected error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> LoadLeavedetails()
        {
            try
            {
                var result = await AdminRepo.GetLeaveDetailsAsync();
                if (result.Rows.Count > 0)
                {
                    var list = new List<ApplyLeave>();
                    foreach (DataRow dr in result.Rows)
                    {
                        var applyLeave = new ApplyLeave
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            LeaveType = Convert.ToString(dr["LeaveType"]),
                            StartDate = DateOnly.FromDateTime(Convert.ToDateTime(dr["StartDate"])),
                            EndDate = DateOnly.FromDateTime(Convert.ToDateTime(dr["EndDate"])),
                            Reason = Convert.ToString(dr["Reason"]),
                            Status = Convert.ToString(dr["Status"])
                        };

                        list.Add(applyLeave);
                    }
                    return Json(new { IsSuccess = true, Message = "", list });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error in LoadLeavedetails");
                return Json(new { IsSuccess = false, Message = "Something went wrong while fetching records: " + ex.Message });
            }

            return Json(new { IsSuccess = false, Message = "No records found" });
        }

        [HttpGet]
        public async Task<ActionResult> GetLeaveDetailById(int id)
        {
            try
            {
                var leaveDetails = await AdminRepo.GetLeaveDetailByIdAsync(id);

                if (leaveDetails != null && leaveDetails.Rows.Count > 0)
                {
                    var row = leaveDetails.Rows[0];

                    var model = new
                    {
                        IsSuccess = true,
                        Id = Convert.ToInt32(row["Id"]),
                        LeaveType = Convert.ToString(row["LeaveType"]),
                        StartDate = DateTime.Parse(Convert.ToString(row["StartDate"])).ToString("yyyy-MM-dd"),
                        EndDate = row["EndDate"] != DBNull.Value ? DateTime.Parse(Convert.ToString(row["EndDate"])).ToString("yyyy-MM-dd") : null,
                        Reason = Convert.ToString(row["Reason"]),
                        Status = Convert.ToString(row["Status"])
                    };

                    return Json(model);
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Leave details not found." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error in GetLeaveDetailById");
                return Json(new { IsSuccess = false, Message = "An error occurred: " + ex.Message });
            }
        }

        public async Task<IActionResult> LoadEmployee()
        {
            try
            {
                var result = await AdminRepo.GetEmployeeAsync();
                if (result.Rows.Count > 0)
                {
                    var list = new List<Employee>();
                    foreach (DataRow dr in result.Rows)
                    {
                        var employee = new Employee
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            UserEmailId = Convert.ToString(dr["UserEmailId"]),
                            Password = Convert.ToString(dr["Password"]),
                            Confirmpassword = Convert.ToString(dr["Confirmpassword"]),
                            Role = Convert.ToInt32(dr["Role"])
                        };

                        list.Add(employee);
                    }
                    return Json(new { IsSuccess = true, Message = "", list });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                // _logger.LogError(ex, "Error in LoadEmployee");
                return Json(new { IsSuccess = false, Message = "Something went wrong while fetching records: " + ex.Message });
            }

            return Json(new { IsSuccess = false, Message = "No records found" });
        }

        [HttpPost]
        public async Task<IActionResult> AddUpdateEmployee([FromBody] Employee employee) // Ensure [FromBody] is used
        {
            try
            {
                if (employee == null)
                {
                    return Json(new { IsSuccess = false, Message = "Employee data is missing or invalid." });
                }

                if (employee.Id == 0)
                {
                    // Add new employee
                    var isAdded = await AdminRepo.AddEmployeeAsync(employee);
                    if (isAdded)
                    {
                        return Json(new { IsSuccess = true, Message = "Employee added successfully" });
                    }
                    else
                    {
                        return Json(new { IsSuccess = false, Message = "Error occurred while adding employee" });
                    }
                }
                else
                {
                    // Update existing employee
                    var isUpdated = await AdminRepo.UpdateEmployeeAsync(employee);
                    if (isUpdated)
                    {
                        return Json(new { IsSuccess = true, Message = "Data updated successfully" });
                    }
                    else
                    {
                        return Json(new { IsSuccess = false, Message = "Error occurred while updating details" });
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { IsSuccess = false, Message = "An unexpected error occurred: " + ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetEmployeeDetailById(int id)
        {
            try
            {
                var employeeDetails = await AdminRepo.GetEmployeeByIdAsync(id);
                if (employeeDetails != null && employeeDetails.Rows.Count > 0)
                {
                    var employee = new Employee
                    {
                        Id = Convert.ToInt32(employeeDetails.Rows[0]["Id"]),
                        Name = Convert.ToString(employeeDetails.Rows[0]["Name"]),
                        UserEmailId = Convert.ToString(employeeDetails.Rows[0]["UserEmailId"]),
                        Password = Convert.ToString(employeeDetails.Rows[0]["Password"]),
                        Confirmpassword = Convert.ToString(employeeDetails.Rows[0]["ConfirmPassword"]),
                        Role = Convert.ToInt32(employeeDetails.Rows[0]["Role"])
                    };

                    return Json(new { IsSuccess = true, Employee = employee });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Employee not found" });
                }
            }
            catch (Exception ex)
            {
                //Log the exception
                return Json(new { IsSuccess = false, Message = "An error occurred: " + ex.Message });
            }
        }
        public async Task<IActionResult> CancelEmployee(int id)
        {
            try
            {
                bool isCanceled = AdminRepo.CancelEmployee(id);

                if (isCanceled)
                {
                    return Json(new { IsSuccess = true, Message = "Leave canceled successfully" });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Failed to cancel leave" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "An error occurred while canceling leave: " + ex.Message });
            }
        }


    }
}
