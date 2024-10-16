using ILeavePortal.Models;
using ILeavePortal.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ILeavePortal.Controllers
{
    public class ApplyLeaveController : Controller
    {
        ApplyLeaveRepo applyLeaverepo = new ApplyLeaveRepo();
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetLeaveDetails(int id)
        {
            try
            {
                var leaveDetails = await ApplyLeaveRepo.GetLeaveByIdAsync(id);
                if (leaveDetails != null && leaveDetails.Rows.Count > 0)
                {
                    var leave = new ApplyLeave
                    {
                        Id = Convert.ToInt32(leaveDetails.Rows[0]["Id"]),
                        LeaveType = Convert.ToString(leaveDetails.Rows[0]["LeaveType"]),
                        // Convert DateTime from the database to DateOnly
                        StartDate = DateOnly.FromDateTime(Convert.ToDateTime(leaveDetails.Rows[0]["StartDate"])),
                        EndDate = DateOnly.FromDateTime(Convert.ToDateTime(leaveDetails.Rows[0]["EndDate"])),
                        Reason = Convert.ToString(leaveDetails.Rows[0]["Reason"]),
                    };

                    return Json(new
                    {
                        IsSuccess = true,
                        ApplyLeave = new
                        {
                            leave.Id,
                            leave.LeaveType,
                            StartDate = leave.StartDate.ToString("yyyy-MM-dd"),  // Format DateOnly for JSON response
                            EndDate = leave.EndDate.ToString("yyyy-MM-dd"),      // Format DateOnly for JSON response
                            leave.Reason
                        }
                    });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Leave not found" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception if applicable
                return Json(new { IsSuccess = false, Message = "An error occurred: " + ex.Message });
            }
        } 


        [HttpPost]
        public IActionResult LoadApplyLeave()
        {
            try
            {
                var result = ApplyLeaveRepo.GetApplyLeave();
                if (result.Rows.Count > 0)
                {
                    var List = new List<ApplyLeave>();
                    foreach (DataRow dr in result.Rows)
                    {
                        ApplyLeave Obj = new ApplyLeave();
                        Obj.Id = Convert.ToInt32(dr["Id"]);
                        Obj.LeaveType = Convert.ToString(dr["LeaveType"]);
                        Obj.StartDate = DateOnly.FromDateTime(Convert.ToDateTime(dr["StartDate"]));
                        Obj.EndDate = DateOnly.FromDateTime(Convert.ToDateTime(dr["EndDate"]));
                        Obj.Reason = Convert.ToString(dr["Reason"]);
                        Obj.Status = Convert.ToString(dr["Status"]);

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


        [HttpPost]
        public async Task<IActionResult> AddUpdateLeave([FromBody] ApplyLeave applyLeave) // Ensure [FromBody] is used
        {
            try
            {
                if (applyLeave == null)
                {
                    return Json(new { IsSuccess = false, Message = "Leave data is missing or invalid." });
                }

                if (applyLeave.Id == 0)
                {
                    // Add new employee
                    var isAdded = await ApplyLeaveRepo.AddLeaveAsync(applyLeave);
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
                    var isUpdated = await ApplyLeaveRepo.UpdateLeaveAsync(applyLeave);
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

        //[HttpPost]
        //public IActionResult EditLeave([FromBody] ApplyLeave updatedLeave)
        //{
        //    try
        //    {
        //        // Assuming ApplyLeaveRepo has a method to update an existing leave request
        //        bool isUpdated = ApplyLeaveRepo.UpdateLeaveAsync(updatedLeave);

        //        if (isUpdated)
        //        {
        //            return Json(new { IsSuccess = true, Message = "Leave updated successfully" });
        //        }
        //        else
        //        {
        //            return Json(new { IsSuccess = false, Message = "Failed to update leave" });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { IsSuccess = false, Message = "An error occurred while updating leave: " + ex.Message });
        //    }
        //}


        [HttpGet]
        public async Task<IActionResult> GetLeaveDetailById(int id)
        {
            try
            {
                var leaveDetails = await ApplyLeaveRepo.GetLeaveByIdAsync(id);
                if (leaveDetails != null && leaveDetails.Rows.Count > 0)
                {
                    var applyLeave = new ApplyLeave
                    {
                        Id = Convert.ToInt32(leaveDetails.Rows[0]["Id"]),
                        LeaveType = Convert.ToString(leaveDetails.Rows[0]["LeaveType"]),
                        StartDate = DateOnly.FromDateTime(Convert.ToDateTime(leaveDetails.Rows[0]["StartDate"])),
                        EndDate = DateOnly.FromDateTime(Convert.ToDateTime(leaveDetails.Rows[0]["EndDate"])),
                        Reason = Convert.ToString(leaveDetails.Rows[0]["Reason"]),
                    };

                    return Json(new { IsSuccess = true, ApplyLeave = applyLeave });
                }
                else
                {
                    return Json(new { IsSuccess = false, Message = "Employee not found" });
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { IsSuccess = false, Message = "An error occurred: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CancelLeave(int id)
        {
            try
            {
                // Assuming ApplyLeaveRepo has a method to delete or mark a leave request as canceled
                bool isCanceled = ApplyLeaveRepo.CancelApplyLeave(id);

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



