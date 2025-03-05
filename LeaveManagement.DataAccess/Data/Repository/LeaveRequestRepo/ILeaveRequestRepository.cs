using LeaveManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.DataAccess.Data.Repository.LeaveRequestRepo
{
    public interface ILeaveRequestRepository
    {
        Task AddAsync(LeaveRequest leaveRequest);
        Task<LeaveRequest?> GetByIdAsync(Guid leaveRequestId);
        Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(string userId);
        Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync();
        Task UpdateAsync(LeaveRequest leaveRequest);
    }
}
