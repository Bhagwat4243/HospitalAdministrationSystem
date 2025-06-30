using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AppModel
{
    public class AppointmentPayment
    {
        [Key]
        public Guid BilingId { get; set; }
        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed
    }
}
