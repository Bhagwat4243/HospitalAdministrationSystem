using HospitalManagementSystem.Db.Model.AuthModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class PrescriptionPdfDto
    {
        public string PatientName { get; set; }         // From ApplicationUser
        public string DoctorName { get; set; }          // From ApplicationUser
        public DateTime Date { get; set; }  
        public string Notes { get; set; }
        public string Diagnosis { get; set; } // Diagnosis information
        public List<PrescriptionMedicinePdfDto> Medicines { get; set; } // List of medicines prescribed
    }
}
