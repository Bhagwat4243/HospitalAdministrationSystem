using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class PatientLabTestDto
    {

        public string Notes { get; set; }
        public List<Guid> LabTestIds { get; set; }
        public DateTime TestDate { get; set; }
        public Guid AppointmentId { get; set; } // Optional, if the lab test is associated with an appointment
    }
}
