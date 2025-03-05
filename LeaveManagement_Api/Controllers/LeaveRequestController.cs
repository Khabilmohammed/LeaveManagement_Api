using LeaveManagement.DataAccess.Services.LeaveRequestServices;
using LeaveManagement.Models.DTO.LeaveRequest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement_Api.Controllers
{
    [Route("api/leave")]
    [ApiController]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        [HttpPost("request")]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] CreateLeaveRequestDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            var leaveRequest = await _leaveRequestService.CreateLeaveRequestAsync(dto);
            return Ok(leaveRequest);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserLeaveRequests(string userId)
        {
            var leaveRequests = await _leaveRequestService.GetUserLeaveRequestsAsync(userId);
            return Ok(leaveRequests);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingLeaveRequests()
        {
            var leaveRequests = await _leaveRequestService.GetAllPendingLeaveRequestsAsync();
            return Ok(leaveRequests);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateLeaveStatus([FromBody] UpdateLeaveStatusDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid data");
            var result = await _leaveRequestService.UpdateLeaveStatusAsync(dto);
            return result ? Ok("Status updated successfully") : NotFound("Leave request not found");
        }
    }
}
