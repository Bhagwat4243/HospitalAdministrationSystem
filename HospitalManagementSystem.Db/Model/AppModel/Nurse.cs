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
    public class Nurse
    {
        [Key]
        public int NurseId { get; set; }
        public string Qualification { get; set; }
        public string RegistrationCouncil { get; set; }
        public string CouncilRegistrationNumber { get; set; }
        public int ExperienceInYears { get; set; }
        public string Departments { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser AppUser { get; set; }
    }
}
