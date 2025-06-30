using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class UpcomingAppointmentDto
    {
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime BookingDate { get; set; }
        public string Status { get; set; }
        public bool IsPaid { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
