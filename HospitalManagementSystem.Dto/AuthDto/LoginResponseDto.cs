using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AuthDto
{
    public class LoginResponseDto
    {
        public UserDto user { get; set; }
        public string Token { get; set; }
    }
}
