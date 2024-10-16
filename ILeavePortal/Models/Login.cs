using System.ComponentModel.DataAnnotations;

namespace ILeavePortal.Models
{
    public class Login
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "UserEmailId is required.")]

        public string UserEmailId{ get; set; }
        [Required(ErrorMessage = "Password is required.")]

        public string Password { get; set; }
     

        //public string Confirmpassword { get; set; }
        //public int Role { get; set; }

    }
}
