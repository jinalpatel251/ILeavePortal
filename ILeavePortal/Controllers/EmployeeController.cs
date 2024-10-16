using ILeavePortal.Models;
using ILeavePortal.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static ILeavePortal.Repository.Employeerepo;

namespace ILeavePortal.Controllers
{
    public class EmployeeController : Controller
    {
         Employeerepo Employeerepo = new Employeerepo();

        public string[] employee { get; private set; }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LoadEmployee()
        {
            try
            {
                var result = Employeerepo.GetEmployee();
                if (result.Rows.Count > 0)
                {
                    var List = new List<Employee>();
                    foreach (DataRow dr in result.Rows)
                    {
                        Employee Obj = new Employee();
                        Obj.Id = Convert.ToInt32(dr["Id"]);
                        Obj.Name = Convert.ToString(dr["Name"]);
                        Obj.UserEmailId = Convert.ToString(dr["UserEmailId"]);
                        Obj.Password = Convert.ToString(dr["Password"]);
                        Obj.Confirmpassword = Convert.ToString(dr["Confirmpassword"]);
                        Obj.Role = Convert.ToInt32(dr["Role"]); // Convert Role to int

                        List.Add(Obj);
                    }
                    return Json(new { IsSuccess = true, Message = "", List });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return Json(new { IsSuccess = false, Message = "Something went wrong while fetching records" });
        }

        public async Task<ActionResult> AddUpdateEmployee()
        {
            try
            {
                string[] employeelist = HttpContext.Request.Form["leavelist[]"];

                //var uploadDirectory = Path.Combine("wwwroot", "VenuImages");

                if (!string.IsNullOrEmpty(employeelist[0]))
                {
                    var result = Employeerepo.FetchLeaveUsingId(employeelist[0]);
                    if (result.Rows.Count > 0)
                    {
                        var resultUpdate = Employeerepo.EditEmployee(employee);
                        if (resultUpdate)
                        {
                            return Json(new { IsSuccess = true, Message = "Data update success" });
                        }
                        else
                        {
                            return Json(new { IsSuccess = false, Message = "Something went wrong while updating details" });
                        }
                    }

                    else
                    {
                        return Json(new { IsSuccess = false, Message = "Error Occurred" });
                    }

                }
                else
                {
                    var resultAdd = Employeerepo.Employee(employeelist);

                    if (resultAdd)
                    {
                        return Json(new { IsSuccess = true, Message = "Success" });
                    }
                    else
                    {
                        return Json(new { IsSuccess = false, Message = "Error Occurred" });
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "An unexpected error occurred" });
            }
            return Json(new { IsSuccess = false, Message = "InValid Request" });
        }

        public IActionResult EditEmployee([FromBody] Employee  updatedemployee)
        {
            try
            {
                // Assuming ApplyLeaveRepo has a method to update an existing leave request
                bool isUpdated = Employeerepo.Update(updatedemployee);

                if (isUpdated)
                {
                    return Json(new { IsSuccess = true, Message = "Employee updated successfully" });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Failed to update employee" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "An error occurred while updating employee: " + ex.Message });
            }
        }

        public IActionResult CancelEmployee(int id)
        {
            try
            {
                // Assuming ApplyLeaveRepo has a method to delete or mark a leave request as canceled
                bool isCanceled = Employeerepo.CancelEmployee(id);

                if (isCanceled)
                {
                    return Json(new { IsSuccess = true, Message = "Employee canceled successfully" });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Failed to cancel employee" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { IsSuccess = false, Message = "An error occurred while canceling leave: " + ex.Message });
            }
        }

    }
}
