using LeaveManagement.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Models.DTO.LeaveRequest
{
    public class UpdateLeaveStatusDTO
    {
        [Required]
        public Guid LeaveRequestId { get; set; }

        [Required]
        public LeaveStatus Status { get; set; } // Approved or Rejected

        public string? ApprovedBy { get; set; } // Manager ID
    }
}
