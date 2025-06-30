using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AuthDto
{
    public class AdminRegistrationDto: RegistrationDto
    {
        [Required(ErrorMessage = "Owner name is required.")]
        [StringLength(100, ErrorMessage = "Owner name can't be longer than 100 characters.")]
        public string OwnerName { get; set; }

        [Required(ErrorMessage = "Owner contact number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Contact number must be between 10 to 15 digits.")]
        public string OwnerContactNo { get; set; }

        [Required(ErrorMessage = "Hospital license is required.")]
        [StringLength(50, ErrorMessage = "License number can't be longer than 50 characters.")]
        public string HospitalLicense { get; set; }

        [Required(ErrorMessage = "GST number is required.")]
        [RegularExpression(@"^\d{2}[A-Z]{5}\d{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", ErrorMessage = "Invalid GST number format.")]
        public string GSTNumber { get; set; }
    }
}
