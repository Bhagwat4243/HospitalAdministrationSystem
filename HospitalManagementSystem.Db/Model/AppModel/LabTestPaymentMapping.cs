using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AppModel
{
    public class LabTestPaymentMapping
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // 🔗 Foreign Key for Payment
        [ForeignKey("LabTestPayment")]
        public Guid PaymentId { get; set; }
        public LabTestPayment LabTestPayment { get; set; }

        // 🔗 Foreign Key for PatientLabTest
        [ForeignKey("PatientLabTest")]
        public Guid PatientLabTestId { get; set; }
        public PatientLabTest PatientLabTest { get; set; }
    }
}
