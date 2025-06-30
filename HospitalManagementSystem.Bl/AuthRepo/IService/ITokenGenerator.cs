using HospitalManagementSystem.Db.Model.AuthModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AuthRepo.IService
{
    public interface ITokenGenerator
    {
        string GenerateToken(ApplicationUser user, IList<string> roles);
    }
}
