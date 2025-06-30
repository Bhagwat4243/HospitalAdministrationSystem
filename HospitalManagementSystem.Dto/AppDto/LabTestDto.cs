using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class LabTestDto
    {
            public Guid AppointmentId { get; set; }  // The appointment associated with the lab test
        public string TestName { get; set; }
            public string PatientName { get; set; }
            public string? Description { get; set; }
            public string SampleRequired { get; set; }  // e.g., Blood, Urine
            public string? Preparation { get; set; }   // e.g., Fasting 12 hrs
            public decimal Cost { get; set; }
            public string? Duration { get; set; }  // e.g., "24 hours"
        
    }
}
