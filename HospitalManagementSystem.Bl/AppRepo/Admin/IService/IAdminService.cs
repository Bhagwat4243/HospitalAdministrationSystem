using HospitalManagementSystem.Dto.AppDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AppRepo.Admin.IService
{
    public interface IAdminService
    {
        Task<List<DoctorDetailsDto>> GetAllDoctor();
        Task<List<PatientDetailsDto>> GetAllPatient();
        Task<List<ChemistDetailsDto>> GetAllChemist();
        Task<List<LabTechnicianDetailsDto>> GetAllLabTechnician();
        Task<List<NurseDetailsDto>> GetAllNurse();
        Task<List<AppointmentDto>> GetAllAppointment();
        Task<LabTestDto>CreateLabTest(LabTestDto labTestDto);
        Task<List<LabTestDto>> GetAllLabTests();
        Task<string> AddNewLabTest(LabTestDto labTestDto);
        Task<List<AppointmentResponseDto>> GetPendingAppointmentsAsync();
        Task<string> ConfirmAppointmentByAdminAsync(Guid appointmentId);
        Task<string> CancelAppointmentByAdminAsync(Guid appointmentId, string? cancelReason = null);
    }
}
