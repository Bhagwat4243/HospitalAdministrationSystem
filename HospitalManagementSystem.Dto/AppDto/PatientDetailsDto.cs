using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class PatientDetailsDto: CommonDetailsDto
    {
        public string BloodGroup { get; set; }
        public string ChronicDiseases { get; set; }
        public string MedicalHistory { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }
    }
}
