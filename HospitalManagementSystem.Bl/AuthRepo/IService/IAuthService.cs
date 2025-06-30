using HospitalManagementSystem.Dto.AuthDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AuthRepo.IService
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginDto loginDto);
        Task<bool> AssignRole(string email, string roleName);
        Task<string> AdminRegistration(AdminRegistrationDto adminRegistrationDto);
        Task<string> DoctorRegistration(DoctorRegistrationDto doctorRegistrationDto);
        Task<string> ChemistRegistration(ChemistRegistrationDto chemistRegistrationDto);
        Task<string> LabTechnicianRegistration(LabTechnicianRegistrationDto labTechnicianRegistrationDto);
        Task<string> NurseRegistration(NurseRegistrationDto nurseRegistrationDto);
        Task<string> RegisterPatient(PatientRegistrationDto patientRegistrationDto);
    }
}
