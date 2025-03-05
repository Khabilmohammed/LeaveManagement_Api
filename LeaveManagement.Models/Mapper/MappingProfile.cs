using AutoMapper;
using LeaveManagement.Models.DTO.LeaveRequest;
using LeaveManagement.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Models.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateLeaveRequestDTO, LeaveRequest>();
            CreateMap<LeaveRequest, LeaveRequestDTO>();
            CreateMap<UpdateLeaveStatusDTO, LeaveRequest>();
        }
    }
}
