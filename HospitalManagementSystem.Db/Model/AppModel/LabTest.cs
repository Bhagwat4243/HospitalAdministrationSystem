using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AppModel
{
    public class LabTest
    {
     
        [Key]
        public Guid LabTestId { get; set; } = Guid.NewGuid();
        public string TestName { get; set; }
        public string? Description { get; set; }
        public string SampleRequired { get; set; }  // e.g., Blood, Urine
        public string? Preparation { get; set; }   // e.g., Fasting 12 hrs
        public decimal Cost { get; set; }
        public string? Duration { get; set; }  // e.g., "24 hours"
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    
        public ICollection<PatientLabTest> PatientLabTests { get; set; }
    }
}
