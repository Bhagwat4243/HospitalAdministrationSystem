using HospitalManagementSystem.Bl.AppRepo.LabTechnician.IService;
using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Dto.AppDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AppRepo.LabTechnician.Implementation
{
    public class LabTechnicianService : ILabTechnicianService
    {
        private readonly HMSDbContext _hMSDbContext;
        public LabTechnicianService(HMSDbContext hMSDbContext)
        {
            _hMSDbContext = hMSDbContext;
        }
    }
}
