using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AuthDto
{
    public class LabTechnicianRegistrationDto: RegistrationDto
    {
        //[Required(ErrorMessage = "Qualification is required.")]
        public List<string> Qualification { get; set; }

        [Required(ErrorMessage = "License number is required.")]
        public string LicenseNumber { get; set; }

        [Required(ErrorMessage = "License issued by is required.")]
        //[StringLength(100, ErrorMessage = "Issuer name can't exceed 100 characters.")]
        public string LicenseIssuedBy { get; set; }

        [Required(ErrorMessage = "License expiry date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime LicenseExpiryDate { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years.")]
        public int ExperienceInYears { get; set; }

        //[Required(ErrorMessage = "Specialization is required.")]
        //[StringLength(100, ErrorMessage = "Specialization can't exceed 100 characters.")]
        public string Specialization { get; set; }

        //[Required(ErrorMessage = "Joining date is required.")]
        //[DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime JoiningDate { get; set; }
    }
}
