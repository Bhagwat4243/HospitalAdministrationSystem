using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AuthDto
{
    public class DoctorRegistrationDto: RegistrationDto
    {
        [Required(ErrorMessage = "Specialization is required.")]
        [StringLength(100, ErrorMessage = "Specialization can't exceed 100 characters.")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Experience is required.")]
        [Range(0, 100, ErrorMessage = "Experience must be between 0 and 100 years.")]
        public int ExperienceYears { get; set; }
        public List<string> Qualifications { get; set; }

        [Phone(ErrorMessage = "Invalid alternate phone number format.")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 to 15 digits.")]
        public string AlternatePhoneNo { get; set; }

        [Required(ErrorMessage = "License number is required.")]
        [StringLength(50, ErrorMessage = "License number can't exceed 50 characters.")]
        public string LicenseNumber { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? LicenseExpiryDate { get; set; }
        public List<string> AvailableDays { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Room number is required.")]
        [StringLength(20, ErrorMessage = "Room number can't exceed 20 characters.")]
        public string RoomNumber { get; set; }
    }
}
