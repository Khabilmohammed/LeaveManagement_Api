using AutoMapper;
using LeaveManagement.DataAccess.Data.Repository.LeaveRequestRepo;
using LeaveManagement.Models.DTO.LeaveRequest;
using LeaveManagement.Models.Models;
using LeaveManagement.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.DataAccess.Services.LeaveRequestServices
{
    public class LeaveRequestService: ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly IMapper _mapper;
        public LeaveRequestService(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            _leaveRequestRepository = leaveRequestRepository;
            _mapper = mapper;
        }

        public async Task<LeaveRequestDTO> CreateLeaveRequestAsync(CreateLeaveRequestDTO dto)
        {
            var leaveRequest = _mapper.Map<LeaveRequest>(dto);
            leaveRequest.Status = LeaveStatus.Pending;
            leaveRequest.AppliedDate = DateTime.UtcNow;

            await _leaveRequestRepository.AddAsync(leaveRequest);
            return _mapper.Map<LeaveRequestDTO>(leaveRequest);
        }

        public async Task<IEnumerable<LeaveRequestDTO>> GetUserLeaveRequestsAsync(string userId)
        {
            var leaveRequests = await _leaveRequestRepository.GetLeaveRequestsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<LeaveRequestDTO>>(leaveRequests);
        }

        public async Task<IEnumerable<LeaveRequestDTO>> GetAllPendingLeaveRequestsAsync()
        {
            var leaveRequests = await _leaveRequestRepository.GetPendingLeaveRequestsAsync();
            return _mapper.Map<IEnumerable<LeaveRequestDTO>>(leaveRequests);
        }

        public async Task<bool> UpdateLeaveStatusAsync(UpdateLeaveStatusDTO dto)
        {
            var leaveRequest = await _leaveRequestRepository.GetByIdAsync(dto.LeaveRequestId);
            if (leaveRequest == null) return false;

            leaveRequest.Status = dto.Status;
            leaveRequest.ApprovedBy = dto.ApprovedBy;
            leaveRequest.ApprovalDate = dto.Status == LeaveStatus.Approved ? DateTime.UtcNow : null;

            await _leaveRequestRepository.UpdateAsync(leaveRequest);
            return true;
        }
    }
}
