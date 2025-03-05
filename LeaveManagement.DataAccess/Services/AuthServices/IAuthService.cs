using LeaveManagement.Models.DTO.LoginAndRegister;
using LeaveManagement.Utility.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.DataAccess.Services.AuthServices
{
    public interface IAuthService
    {
        Task<APIResponse> RegisterAsync(RegisterRequestDTO model);
        Task<APIResponse> LoginAsync(LoginRequestDTO model);
    }
}
