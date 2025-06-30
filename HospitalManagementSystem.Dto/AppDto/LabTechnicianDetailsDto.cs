using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class LabTechnicianDetailsDto: CommonDetailsDto
    {
        public int LabTechnicianId { get; set; }
        public string Qualification { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseIssuedBy { get; set; }
        public DateTime LicenseExpiryDate { get; set; }
        public int ExperienceInYears { get; set; }
        public string Specialization { get; set; }
        public DateTime JoiningDate { get; set; }

        public string UserId { get; set; }
    }
}
