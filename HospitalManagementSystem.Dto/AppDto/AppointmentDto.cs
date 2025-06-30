using HospitalManagementSystem.Db.Model.AppModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class AppointmentDto
    {
        public string DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }     // Enum preferred

        public string Remark { get; set; }
        public Guid AppointmentId { get; set; }
        public string PatientId { get; set; }
        public DateTime AppointmentTime { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public AppointmentStatus Status { get; set; }

        public string StatusText => Status.ToString();
    }
}
