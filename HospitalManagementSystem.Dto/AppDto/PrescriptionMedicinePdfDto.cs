using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class PrescriptionMedicinePdfDto
    {
        public string Medicine { get; set; }
        public string Dosage { get; set; }
        public string Frequancy { get; set; }
        public string Duration { get; set; }
        public string Instructions { get; set; }
    }
}
