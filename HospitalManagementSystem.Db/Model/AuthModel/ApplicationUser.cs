
using HospitalManagementSystem.Db.Model.AppModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AuthModel
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string AddharNo { get; set; }
        public DateOnly DOB { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public ICollection<Appointment> PatientAppointments { get; set; }
        public ICollection<Appointment> DoctorAppointments { get; set; }
    }
}
