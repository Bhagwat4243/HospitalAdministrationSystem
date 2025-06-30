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
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }
        public string Specialization { get; set; }
        public int ExperienceYears { get; set; }
        public string Qualifications { get; set; }
        public string AlternatePhoneNo { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime? LicenseExpiryDate { get; set; }
        public string AvailableDays { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string RoomNumber { get; set; }
        public decimal ConsultantFee { get; set; }
        public bool IsActive { get; set; } = true; // Default to true, indicating the doctor is active

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser AppUser { get; set; }
    }
}
