using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Models.DTO.LoginAndRegister
{
    public class RegisterRequestDTO
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [RegularExpression("^(Manager|Employee)$", ErrorMessage = "Role must be either 'Manager' or 'Employee'.")]
        public string Role { get; set; }    
    }
}
