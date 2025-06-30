using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class ViewPrescriptionDto
    {
        public Guid ViewPrescriptionId { get; set; }
        public DateTime PrescribeDate { get; set; }
        public string Notes { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public List<PrescriptionDetailsDto> ViewPrescriptionDetails { get; set; }

    }
}
