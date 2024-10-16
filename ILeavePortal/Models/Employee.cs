using System.ComponentModel.DataAnnotations;

namespace ILeavePortal.Models
{
    public class Employee
    {
       public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]

        public string Name { get; set; }
        [Required(ErrorMessage = "UserEmailId is required.")]

        public string UserEmailId { get; set; } // Add this property
        [Required(ErrorMessage = "Password is required.")]

        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword is required.")]

        public string Confirmpassword { get; set; } 
        public int Role { get; set; }

       
    }
}
