using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class PrescriptionDto
    {
        public Guid AppointmentId { get; set; }
        public string? Notes { get; set; }
        public List<PrescriptionDetailsDto> Details { get; set; }
    }
}
