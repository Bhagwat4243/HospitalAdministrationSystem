using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class NurseDetailsDto: CommonDetailsDto
    {
        public int NurseId { get; set; }
        public string Qualification { get; set; }
        public string RegistrationCouncil { get; set; }
        public string CouncilRegistrationNumber { get; set; }
        public int ExperienceInYears { get; set; }
        public string Departments { get; set; }

        public string UserId { get; set; }
    }
}
