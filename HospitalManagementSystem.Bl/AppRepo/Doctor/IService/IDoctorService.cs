using HospitalManagementSystem.Dto.AppDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AppRepo.Doctor.IService
{
    public interface IDoctorService
    {
        Task<PrescriptionDto>CreatePrescription(PrescriptionDto prescriptionDto);
        Task<string> PrescribeLabTestsAsync(PatientLabTestDto patientLabTestDto,string doctorId);
        Task<PrescriptionDto>UpdatePrescription(PrescriptionDto prescriptionDto, Guid PrescriptionId);
        Task<List<PatientDetailsDto>> GetAllPatientsAssignedToDoctor(string doctorId);
        Task<PatientDetailsDto> GetPatientDetailsById(string patientId);
        Task<PrescriptionDto> GetPatientPrescription(string patientId);
        Task<List<LabTestDto>> GetLabTests();
    }
}
