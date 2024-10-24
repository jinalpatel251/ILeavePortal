using System.ComponentModel.DataAnnotations;

namespace ILeavePortal.Models
{
    public class Login
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Email ID is required.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string UserEmailId{ get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d).+$", ErrorMessage = "Password must contain at least one letter and one number.")]
        public string Password { get; set; }
     

        //public string Confirmpassword { get; set; }
        //public int Role { get; set; }

    }
}
