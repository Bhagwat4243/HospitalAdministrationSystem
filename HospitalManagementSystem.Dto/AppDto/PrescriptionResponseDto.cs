using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class PrescriptionResponseDto
    {
        public Guid PrescriptionId { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime PrescribeDate { get; set; }
        public string DoctorName { get; set; }
        public string PatientName { get; set; }
        public List<MedicineInfoResponseDto> MedicinesInfoDto { get; set; }
    }
}
