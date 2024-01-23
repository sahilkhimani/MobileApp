using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INOVI.DTO
{
    public class UserDTO
    {
    }
    public class LoginDTO
    {
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }

    public class SignupDTO
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
    }

    public class UpdateUserPasswordDTO
    {
        public string UserEmail { get; set; }
        public string Otp { get; set; }
        public string UserPassword { get; set; }
        public string UserConfirmPassword { get; set; }

    }

    public class SendOTPDTO
    {
        public string UserEmail { get; set; }
    }
    
    public class ValidateOTPDTO
    {
        public string UserEmail { get; set; }
        public string Otp { get; set; }
    }

    public class GetUserDTO
    {
        public int UserID { get; set; }
        public int? RoleID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
