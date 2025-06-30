using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AuthDto
{
    public class NurseRegistrationDto: RegistrationDto
    {
        //[Required(ErrorMessage = "Qualification is required.")]
        //[StringLength(100, ErrorMessage = "Qualification can't exceed 100 characters.")]
        public List<string> Qualification { get; set; }

        //[Required(ErrorMessage = "Registration council is required.")]
        //[StringLength(100, ErrorMessage = "Registration council name can't exceed 100 characters.")]
        public string RegistrationCouncil { get; set; }

        //[Required(ErrorMessage = "Council registration number is required.")]
        //[StringLength(50, ErrorMessage = "Council registration number can't exceed 50 characters.")]
        public string CouncilRegistrationNumber { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years.")]
        public int ExperienceInYears { get; set; }

        //[Required(ErrorMessage = "Departments are required.")]
        //[StringLength(200, ErrorMessage = "Departments can't exceed 200 characters.")]
        public string Departments { get; set; }
    }
}
