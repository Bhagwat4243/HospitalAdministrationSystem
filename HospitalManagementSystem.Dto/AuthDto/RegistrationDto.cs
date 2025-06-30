using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AuthDto
{
    public class RegistrationDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number.")]
        public string PhoneNo { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string AddharNo { get; set; }

        public DateOnly DateOfBirth { get; set; }
        [Required(ErrorMessage = "Postal code is required.")]

        public string PostalCode { get; set; }

        [Required(ErrorMessage = "At least one role must be assigned.")]
        [MinLength(1, ErrorMessage = "At least one role must be provided.")]
        public List<string> Role { get; set; }
    }
}
