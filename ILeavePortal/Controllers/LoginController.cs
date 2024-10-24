using ILeavePortal.Models;
using ILeavePortal.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;


namespace ILeavePortal.Controllers
{
    public class LoginController : Controller
    {
        Loginrepo loginrepo = new Loginrepo();
        Employeerepo employeerepo = new Employeerepo();
        public IActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            return Json(loginrepo.ListAll());
        }

        [HttpPost]
        public IActionResult Add(Employee login)
        {
            int result = loginrepo.Add(login);

            if (result > 0)
            {
                // Create a cookie to store UserEmailId
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(7); // Set expiration for the cookie

                Response.Cookies.Append("UserEmailId", login.UserEmailId, options);

                if (login.UserEmailId == "admin@gmail.com")
                {
                    return Json(new { success = true, RedirectUrl = "/Login/Index" });
                }
                if (employeerepo.CheckEmailExists(login.UserEmailId,login.Password))
                {
                    return Json(new { success = true, RedirectUrl = "/Employee/Index" });
                }
                
                else
                {
                    return Json(new { success = false, errors = new List<string> { "User not found in records." } });
                }
            }

            return View("ErrorPage");
        }


        [HttpPost]
        public JsonResult AddEmployee(Employee employee)
        {
            loginrepo.AddEmployee(employee);
            return Json(new { success = true, RedirectUrl = "/Login/Index" });
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpGet]
        public IActionResult UpdateEmployee()
        {

            return View();
        }


        [HttpGet]
        public JsonResult GetbyId(int Id)
        {
            var Employee = loginrepo.ListAll().Find(x => x.Id.Equals(Id));
            return Json(Employee);
            //return Json (new { success = true, RedirectUrl = "/Login/UpdateEmployee?"+Id,Employee });
        }
        [HttpPost]
        public JsonResult UpdateEmployee(Employee employee)
        {
            loginrepo.UpdateEmployee(employee);
            return Json(new { success = true, RedirectUrl = "/Login/Index" });
        }
        public JsonResult Delete(int Id)

        {
            return Json(loginrepo.Delete(Id));
        }



    }
}


