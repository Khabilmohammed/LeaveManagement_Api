using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Utility
{
    public class SD
    {

        public const string Role_Manager = "Manager";
        public const string Role_Employee = "Employee";

    }

    public enum LeaveType
    {
        SickLeave,
        CasualLeave,
        PaidLeave,
        UnpaidLeave
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
