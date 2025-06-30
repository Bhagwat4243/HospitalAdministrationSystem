using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AuthDto
{
    public class ChemistRegistrationDto: RegistrationDto
    {

        [Required(ErrorMessage = "Qualification is required.")]
        public List<string> Qualification { get; set; }

        [Required(ErrorMessage = "License number is required.")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "License issued by field is required.")]
        [StringLength(100, ErrorMessage = "Issuer name can't exceed 100 characters.")]
        public string LicenseIssuedBy { get; set; }

        public DateTime LicenseExpiryDate { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years.")]
        public int ExperienceInYears { get; set; }
    }
}
