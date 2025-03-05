using LeaveManagement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Models.DTO.LeaveRequest
{
    public class CreateLeaveRequestDTO
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public LeaveType LeaveType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [MaxLength(700)]
        public string Reason { get; set; }
    }
}
