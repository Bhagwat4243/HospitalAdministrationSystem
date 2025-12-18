using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class LabTestStartResponseDto
    {

        public Guid LabTestId { get; set; }
        public string TestName { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public DateTime StartedAt { get; set; }
        public string Status { get; set; }
    }
}
