using HospitalManagementSystem.Db.Model.AuthModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AppModel
{
    public class PatientLabTest
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public Appointment Appointment { get; set; }


        public string PatientId { get; set; }              // FK to ApplicationUser
        public ApplicationUser Patient { get; set; }

        public Guid LabTestId { get; set; }                // FK to LabTest
        public LabTest LabTest { get; set; }
        [ForeignKey("DoctorId")]
        public string DoctorId { get; set; }           // FK to Doctor (ApplicationUser)
        public ApplicationUser? Doctor { get; set; }

        public DateTime TestDate { get; set; }
        public string? Notes { get; set; } // Additional notes or instructions for the lab test
        public string? Result { get; set; } // Result of the lab test, if available
        public string? ReportFilePath { get; set; } // Path to the report file, if applicable
        public LabTestStatus Status { get; set; } = LabTestStatus.Requested;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;

    }
    public enum LabTestStatus
    {
        Requested,
        InProgress,
        Completed,
        Cancelled
    }
}
