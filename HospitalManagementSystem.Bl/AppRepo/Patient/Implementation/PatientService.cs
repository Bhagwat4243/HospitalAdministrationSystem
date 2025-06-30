using HospitalManagementSystem.Bl.AppRepo.Patient.IService;
using HospitalManagementSystem.Bl.PdfGeneratorFolder.IService;
using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Dto.AppDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AppRepo.Patient.Implementation
{
    public class PatientService : IPatientService
    {
        private readonly HMSDbContext _hMSDbContext;
        private readonly IPdfGenerate _pdfgenerate;
        public PatientService(HMSDbContext hMSDbContext,IPdfGenerate pdfGenerate)
        {
            _hMSDbContext = hMSDbContext;
            _pdfgenerate = pdfGenerate;
        }

        public async Task<UpcomingAppointmentDto> ShowBookAppoint()
        {
            try
            {
                var upcomingAppointments = await _hMSDbContext.Appointment_Tbl
                    .Where(a => a.Status == AppointmentStatus.Pending || a.Status == AppointmentStatus.Confirmed)
                    .Select(a => new UpcomingAppointmentDto
                    {
                        AppointmentDate = a.AppointmentDate,
                        BookingDate = a.BookingDate,
                         DoctorName = _hMSDbContext.Users
                            .Where(u => u.Id == a.DoctorId)
                            .Select(u => u.Name)
                            .FirstOrDefault(),
                        Status = a.Status.ToString(),
                        IsPaid = _hMSDbContext.AppointmentPayment_Tbl
                            .Any(p => p.AppointmentId == a.AppointmentId && p.PaymentStatus == PaymentStatus.Paid),
                        PaidAmount = _hMSDbContext.AppointmentPayment_Tbl
                            .Where(p => p.AppointmentId == a.AppointmentId)
                            .Select(p => p.PaidAmount)
                            .FirstOrDefault(),

                        TotalAmount = _hMSDbContext.AppointmentPayment_Tbl
                            .Where(p => p.AppointmentId == a.AppointmentId)
                            .Select(p => p.TotalAmount)
                            .FirstOrDefault()

                    })
                    .ToListAsync();
                if (upcomingAppointments == null || !upcomingAppointments.Any())
                {
                    throw new Exception("No upcoming appointments found.");
                }
                return upcomingAppointments.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("An error occurred while creating the appointment.", ex);
            }
        }

        public async Task<string> BookNewAppointment(BookAppointmentDto bookAppointmentDto,string patientId)
        {
            try
            {
                if (bookAppointmentDto == null)
                {
                    throw new ArgumentNullException("Appointment details cannot be null.");
                }
                var isDoctorIdExists = await _hMSDbContext.Doctor_Tbl.AnyAsync(d => d.UserId == bookAppointmentDto.DoctorId);
                if (!isDoctorIdExists)
                {
                    throw new Exception("Doctor with the provided ID does not exist.");
                }
                var newAppointment = new Appointment
                {
                    DoctorId = bookAppointmentDto.DoctorId,
                    PatientId = patientId,
                    AppointmentDate = bookAppointmentDto.AppointmentDate,
                    Remark = bookAppointmentDto.Remark,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    BookingDate = DateTime.Now,
                    Status = AppointmentStatus.Pending // Default status when booking
                };
                await _hMSDbContext.Appointment_Tbl.AddAsync(newAppointment);
                await _hMSDbContext.SaveChangesAsync();

                return "Appointment booked successfully.";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return "An error occurred while booking the appointment.";
            }
        }

        public async Task<AppointmentPaymentDto> CreateAppointPayment(AppointmentPaymentDto appointmentPaymentDto)
        {
            try
            {
                if (appointmentPaymentDto == null)
                    throw new ArgumentNullException("Payment details cannot be null.");

                // Join Appointment_Tbl and Doctor_data_Tbl to get doctor fee
                var appointmentInfo = await (
                    from appointment in _hMSDbContext.Appointment_Tbl
                    join doctor in _hMSDbContext.Users on appointment.DoctorId equals doctor.Id
                    join doctorData in _hMSDbContext.Doctor_Tbl on doctor.Id equals doctorData.UserId
                    where appointment.AppointmentId == appointmentPaymentDto.AppointmentId
                    select new
                    {
                        appointment.AppointmentId,
                        doctorData.ConsultantFee
                    }
                ).FirstOrDefaultAsync();

                if (appointmentInfo == null)
                    throw new Exception("Appointment not found.");

                // Create payment entity
                var payment = new AppointmentPayment
                {
                    AppointmentId = appointmentInfo.AppointmentId,
                    TotalAmount = appointmentInfo.ConsultantFee,
                    PaidAmount = appointmentPaymentDto.Ammount,
                    PaymentStatus = PaymentStatus.Paid,
                    PaymentDate = DateTime.Now
                };

                await _hMSDbContext.AppointmentPayment_Tbl.AddAsync(payment);
                await _hMSDbContext.SaveChangesAsync();

                return new AppointmentPaymentDto
                {
                    AppointmentId = payment.AppointmentId,
                    Ammount = payment.TotalAmount,
                    
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<DoctorDtoForPatientView> GetDoctorDetails(string Name = null, string Specialization = null)
        {
            try
            {
                var query = _hMSDbContext.Doctor_Tbl
                       .Include(d => d.AppUser)
                       .Where(d => d.IsActive); // Filter for active doctors
                if (!string.IsNullOrEmpty(Name))
                {
                    query = query.Where(d => d.AppUser.Name.Contains(Name));
                }

                if (!string.IsNullOrEmpty(Specialization))
                {
                    query = query.Where(d => d.Specialization.Contains(Specialization));
                }

                var doctorEntity = await query.FirstOrDefaultAsync();
                if (doctorEntity == null) throw new Exception("No doctor found");

                var doctor = new DoctorDtoForPatientView
                {
                    DoctorId = doctorEntity.UserId,
                    Name = doctorEntity.AppUser.Name,
                    Email = doctorEntity.AppUser.Email,
                    State = doctorEntity.AppUser.State,
                    City = doctorEntity.AppUser.City,
                    Specialization = doctorEntity.Specialization,
                    StartTime = doctorEntity.StartTime,
                    EndTime = doctorEntity.EndTime,
                    Qualifications = await _hMSDbContext.Doctor_Tbl
                                        .Where(q => q.DoctorId == doctorEntity.DoctorId)
                                        .Select(q => q.Qualifications)
                                        .ToListAsync(),
                    AvailableDays = await _hMSDbContext.Doctor_Tbl
                                        .Where(d => d.DoctorId == doctorEntity.DoctorId)
                                        .Select(d => d.AvailableDays)
                                        .ToListAsync()
                };
                return doctor;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving doctor details.", ex);
            }
        }
        public async Task<List<ViewPrescriptionDto>> GetPrescriptionList()
        {
            try
            {
                var prescriptions = await _hMSDbContext.Prescription_Tbl
                    .Include(p => p.Appointment)
                        .ThenInclude(a => a.Patient)
                    .Include(p => p.Appointment)
                        .ThenInclude(a => a.Doctor)
                    .Select(p => new ViewPrescriptionDto
                    {
                        PrescribeDate = p.PrescriptionDate,
                        DoctorName = p.Appointment.Doctor.Name,
                        AppointmentDate = p.Appointment.AppointmentDate,
                        Notes = p.Notes,
                        ViewPrescriptionDetails = _hMSDbContext.PrescriptionDetail_Tbl
                            .Where(pd => pd.PrescriptionId == p.PrescriptionId)
                            .Select(pd => new PrescriptionDetailsDto
                            {
                                Medicine = pd.MedicineName,
                                Dosage = pd.Dosage,
                                Frequency = pd.Frequency,
                                Duration = pd.Duration,
                                Instructions = pd.Instructions
                            }).ToList()
                    })
                    .ToListAsync();

                if (prescriptions == null || !prescriptions.Any())
                    throw new Exception("No prescriptions found.");

                return prescriptions;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the prescription list.", ex);
            }
        }

        public async Task<List<AppointmentDto>> GetAppointmentByPatientId(string patientId)
        {

                if (string.IsNullOrWhiteSpace(patientId))
                    throw new ArgumentException("Patient ID cannot be null or empty.");

                try
                {
                    var appointments = await _hMSDbContext.Appointment_Tbl
                        .Where(a => a.PatientId == patientId)
                        .Include(a => a.Patient)
                        .Include(a => a.Doctor)
                        .Select(a => new AppointmentDto
                        {
                            AppointmentId = a.AppointmentId,
                            DoctorId = a.DoctorId,
                            PatientId = a.PatientId,
                            AppointmentDate = a.AppointmentDate,
                            AppointmentTime = a.AppointmentDate.TimeOfDay != null ? a.AppointmentDate : a.AppointmentDate,
                            Remark = a.Remark,
                            Status = a.Status,
                            PatientName = a.Patient != null ? a.Patient.Name : "Unknown",
                            DoctorName = a.Doctor != null ? a.Doctor.Name : "Unknown"
                        })
                        .ToListAsync();

                    return appointments;
                }
                catch (Exception ex)
                {
                    // Log the exception (not implemented here)
                    throw new Exception("An error occurred while fetching appointments for the patient.", ex);
                }
        }

        public async Task<byte[]> DownloadPrescriptionPdf(Guid prescriptionId, string paientId)
        {
            try
            {
                if (prescriptionId == Guid.Empty)
                    throw new ArgumentException("Invalid prescription ID.");
                var prescription = await _hMSDbContext.Prescription_Tbl
                    .Include(p => p.Details)
                    .Include(p => p.Appointment)
                        .ThenInclude(a => a.Patient)
                    .Include(p => p.Appointment)
                        .ThenInclude(a => a.Doctor)
                    .FirstOrDefaultAsync(p => p.PrescriptionId == prescriptionId && p.Appointment.PatientId == paientId);
                if (prescription == null)
                    throw new Exception("Prescription not found or does not belong to the specified patient.");
                // Assuming you have a method to generate PDF from prescription data
                // This is a placeholder for actual PDF generation logic

                var duagnosis= _hMSDbContext.Patient_Tbl
                    .Where(p => p.UserId == paientId)
                    .Select(p => p.ChronicDiseases)
                    .FirstOrDefault();
                var dto = new PrescriptionPdfDto
                {
                    PatientName = prescription.Appointment.Patient.Name,
                    DoctorName = prescription.Appointment.Doctor.Name,
                    Diagnosis = duagnosis,
                    Notes = prescription.Notes,
                    Date = prescription.PrescriptionDate,
                    Medicines = prescription.Details.Select(d => new PrescriptionMedicinePdfDto
                    {
                        Medicine = d.MedicineName,
                        Dosage = d.Dosage,
                        Frequancy = d.Frequency,
                        Duration = d.Duration,
                        Instructions = d.Instructions
                    }).ToList()
                };
                byte[] pdfData = _pdfgenerate.GeneratePrescriptionPdf(dto);
                return pdfData;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while downloading the prescription PDF.", ex);
            }
        }
    }
}
