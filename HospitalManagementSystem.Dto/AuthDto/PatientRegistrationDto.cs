using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AuthDto
{
    public class PatientRegistrationDto: RegistrationDto
    {
        [Required(ErrorMessage = "Blood group is required.")]
        [RegularExpression(@"^(A|B|AB|O)[+-]$", ErrorMessage = "Invalid blood group format (e.g., A+, B-, O+).")]
        public string BloodGroup { get; set; }

        [StringLength(500, ErrorMessage = "Chronic diseases info can't exceed 500 characters.")]
        public string ChronicDiseases { get; set; }

        [StringLength(1000, ErrorMessage = "Medical history can't exceed 1000 characters.")]
        public string MedicalHistory { get; set; }

        [Required(ErrorMessage = "Registration date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    }
}
