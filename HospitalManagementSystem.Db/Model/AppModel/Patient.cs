using HospitalManagementSystem.Db.Model.AuthModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AppModel
{
    public class Patient
    {
        [Key]
        public int PatientId { get; set; }
        public string BloodGroup { get; set; }
        public string ChronicDiseases { get; set; }
        public string MedicalHistory { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser AppUser { get; set; }
    }
}
