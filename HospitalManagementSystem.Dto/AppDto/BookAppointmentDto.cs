using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class BookAppointmentDto
    {
        public DateTime AppointmentDate { get; set; }
        public string DoctorId { get; set; }  // FK to ApplicationUser.Id (Doctor)
        public string Remark { get; set; } 
    }
}
