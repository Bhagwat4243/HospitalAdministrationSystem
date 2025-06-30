using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AppModel
{
    public class PrescriptionDetails
    {
        public Guid Id { get; set; }
       
        public Guid PrescriptionId { get; set; }
        public Prescriptions Prescription { get; set; }
        public string MedicineName { get; set; }
        public string Dosage { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }
        public string? Instructions { get; set; }
    }
}
