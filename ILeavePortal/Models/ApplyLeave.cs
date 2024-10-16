using System.ComponentModel.DataAnnotations;

namespace ILeavePortal.Models
{
    public class ApplyLeave
    {
        public int Id { get; set; }
        public string LeaveType { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly StartDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateOnly EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }

        //public int? LoginId { get; set; }
    }
}
