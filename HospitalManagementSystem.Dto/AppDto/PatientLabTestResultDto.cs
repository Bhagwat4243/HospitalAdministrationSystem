using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class PatientLabTestResultDto
    {
        public Guid LabTestId { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public string Result { get; set; }
        public string? ReportFileUrl { get; set; }
        public string DoctorName { get; set; }
        public string Status { get; set; }
    }
}
