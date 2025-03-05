using LeaveManagement.Models.DTO.LeaveRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.DataAccess.Services.LeaveRequestServices
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequestDTO> CreateLeaveRequestAsync(CreateLeaveRequestDTO dto);
        Task<IEnumerable<LeaveRequestDTO>> GetUserLeaveRequestsAsync(string userId);
        Task<IEnumerable<LeaveRequestDTO>> GetAllPendingLeaveRequestsAsync();
        Task<bool> UpdateLeaveStatusAsync(UpdateLeaveStatusDTO dto);
    }
}
