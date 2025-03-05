using LeaveManagement.Models.Models;
using LeaveManagement.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.DataAccess.Data.Repository.LeaveRequestRepo
{
    public class LeaveRequestRepository:ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;
        public LeaveRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LeaveRequest leaveRequest)
        {
            await _context.LeaveRequests.AddAsync(leaveRequest);
            await _context.SaveChangesAsync();
        }

        public async Task<LeaveRequest?> GetByIdAsync(Guid leaveRequestId)
        {
            return await _context.LeaveRequests.FindAsync(leaveRequestId);
        }


        public async Task<IEnumerable<LeaveRequest>> GetLeaveRequestsByUserIdAsync(string userId)
        {
            return await _context.LeaveRequests.Where(lr => lr.UserId == userId).ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetPendingLeaveRequestsAsync()
        {
            return await _context.LeaveRequests.Where(lr => lr.Status == LeaveStatus.Pending).ToListAsync();
        }
        public async Task UpdateAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            await _context.SaveChangesAsync();
        }
    }
}
