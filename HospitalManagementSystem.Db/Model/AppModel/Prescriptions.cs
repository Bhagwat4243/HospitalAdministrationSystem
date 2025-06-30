using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AppModel
{
    public class Prescriptions
    {
        [Key]
        public Guid PrescriptionId { get; set; }
        public Guid AppointmentId { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public string? Notes { get; set; }
        public Appointment Appointment { get; set; }
        public ICollection<PrescriptionDetails> Details { get; set; }
    }
}
