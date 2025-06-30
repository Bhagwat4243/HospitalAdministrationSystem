using HospitalManagementSystem.Db.Model.AuthModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Db.Model.AppModel
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerContactNo { get; set; }
        public string HospitalLicense { get; set; }
        public string GSTNumber { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser AppUser { get; set; }
    }
}
