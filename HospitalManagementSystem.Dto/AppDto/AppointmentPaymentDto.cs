using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace HospitalManagementSystem.Dto.AppDto
{
    public class AppointmentPaymentDto
    {
        public Guid AppointmentId { get; set; }
        public decimal Ammount { get; set; }
    }
}