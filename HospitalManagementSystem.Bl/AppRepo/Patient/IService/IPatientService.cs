using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Dto.AppDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AppRepo.Patient.IService
{
    public interface IPatientService
    {
        Task<string> BookNewAppointment(BookAppointmentDto bookAppointmentDto,string patientId);
        Task<AppointmentPaymentDto> CreateAppointPayment(AppointmentPaymentDto appointmentPaymentDto);
        Task<DoctorDtoForPatientView> GetDoctorDetails(string Name=null,string Specialization=null);
        Task<UpcomingAppointmentDto> ShowBookAppoint();
        Task<List<ViewPrescriptionDto>> GetPrescriptionList();
        Task<List<AppointmentDto>>GetAppointmentByPatientId(string patientId);
        Task<byte[]> DownloadPrescriptionPdf(Guid prescriptionId,string paientId);

    }
}
