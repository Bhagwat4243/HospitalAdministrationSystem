using AutoMapper;
using HospitalManagementSystem.Bl.AppRepo.Patient.IService;
using HospitalManagementSystem.Bl.AuthRepo.IService;
using HospitalManagementSystem.Bl.PdfGeneratorFolder.IService;
using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Dto.AppDto;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PatientService(HMSDbContext hMSDbContext,IPdfGenerate pdfGenerate, IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _hMSDbContext = hMSDbContext;
            _pdfgenerate = pdfGenerate;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<PendingLabTestListDto> GetPendingLabTestsForPaymentAsync(string patientId)
        {
            try
            {

                // ✅ Get all lab tests with Requested status for patient
                var labTests = await _hMSDbContext.PatientLabTest_Tbl
                    .Include(x => x.LabTest)
                    .Include(x => x.Doctor)
                    .Include(x => x.Patient)
                    .Where(x => x.PatientId == patientId && x.Status == LabTestStatus.Requested)
                    .ToListAsync();

                if (labTests == null || !labTests.Any())
                    throw new Exception("No pending lab tests found for payment.");

                // ✅ Prepare DTO
                var dto = new PendingLabTestListDto
                {
                    PatientId = patientId,
                    PatientName = labTests.First().Patient.Name,
                    DoctorName = labTests.DefaultIfEmpty().FirstOrDefault()?.Doctor?.Name ?? "N/A",
                    TotalAmount = labTests.Sum(x => x.LabTest.Cost),
                    Tests = labTests.Select(x => new PendingLabTestItemDto
                    {
                        PatientLabTestId = x.Id,
                        LabTestId = x.LabTestId,
                        TestName = x.LabTest.TestName,
                        SampleRequired = x.LabTest.SampleRequired,
                        Preparation = x.LabTest.Preparation,
                        TestDate = x.TestDate,
                        Cost = x.LabTest.Cost,
                    }).ToList()
                };

                return dto;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while retrieving pending lab tests for payment.", ex);
            }
        }

        public async Task<string> PayForLabTestsAsync(LabTestPaymentRequestDto dto, string patientId)
        {
            try
            {

                if (dto == null || !dto.PatientLabTestIds.Any())
                    throw new ArgumentException("No lab tests selected for payment.");

                // ✅ Get relevant lab tests
                var labTests = await _hMSDbContext.PatientLabTest_Tbl
                    .Include(x => x.LabTest)
                    .Include(x => x.Patient)
                    .Where(x => dto.PatientLabTestIds.Contains(x.Id) &&
                                x.PatientId == patientId &&
                                x.Status == LabTestStatus.Requested)
                    .ToListAsync();

                if (labTests.Count != dto.PatientLabTestIds.Count)
                    throw new Exception("Some lab tests are invalid or already completed.");

                var totalCost = labTests.Sum(x => x.LabTest.Cost);

                // ✅ Find previous payments
                var previousPaymentIds = await _hMSDbContext.LabTestPaymentMapping_Tbl
                    .Where(m => dto.PatientLabTestIds.Contains(m.PatientLabTestId))
                    .Select(m => m.PaymentId)
                    .Distinct()
                    .ToListAsync();

                var previousPaidAmount = await _hMSDbContext.LabTestPayment_Tbl
                    .Where(p => previousPaymentIds.Contains(p.PaymentId))
                    .SumAsync(p => p.PaidAmount);

                var cumulativePaid = previousPaidAmount + dto.PaidAmount;
                var remainingAmount = totalCost - cumulativePaid;

                // ✅ Create new payment
                var payment = new LabTestPayment
                {
                    TotalAmount = totalCost,
                    PaidAmount = dto.PaidAmount,
                    IsPaid = cumulativePaid >= totalCost,
                    PaymentStatus = cumulativePaid >= totalCost ? LabPaymentStatus.Successful : LabPaymentStatus.Pending,
                    PaymentDate = DateTime.UtcNow
                };

                await _hMSDbContext.LabTestPayment_Tbl.AddAsync(payment);
                await _hMSDbContext.SaveChangesAsync();

                // ✅ Add new mappings
                var mappings = labTests.Select(t => new LabTestPaymentMapping
                {
                    PatientLabTestId = t.Id,
                    PaymentId = payment.PaymentId
                }).ToList();

                await _hMSDbContext.LabTestPaymentMapping_Tbl.AddRangeAsync(mappings);

                // ✅ Update lab test statuses
                foreach (var test in labTests)
                {
                    if (cumulativePaid >= totalCost)
                        test.Status = LabTestStatus.Paid;

                    test.UpdatedAt = DateTime.UtcNow;
                }

                await _hMSDbContext.SaveChangesAsync();

                // ✅ Email logic
                var patient = labTests.First().Patient;
                string subject, body;

                if (cumulativePaid >= totalCost)
                {
                    subject = "Lab Test Payment Confirmation";
                    body = $@"
                    <p>Dear <b>{patient.Name}</b>,</p>
                    <p>We have successfully received your full payment of ₹{cumulativePaid} for the following lab test(s):</p>
                    <ul>{string.Join("", labTests.Select(x => $"<li>{x.LabTest.TestName} - ₹{x.LabTest.Cost}</li>"))}</ul>
                    <p>Your tests are now scheduled and will be processed soon.</p>
                    <p>Thank you for trusting our services.</p>
                    <br/>
                    <p>Regards,<br/>Hospital Management System</p>";
                }
                else
                {
                    subject = "Partial Lab Test Payment Received";
                    body = $@"
                    <p>Dear <b>{patient.Name}</b>,</p>
                    <p>We have received ₹{dto.PaidAmount} towards your lab test(s), out of a total ₹{totalCost}.</p>
                    <p><b>Total Paid So Far:</b> ₹{cumulativePaid}<br/><b>Remaining Balance:</b> ₹{remainingAmount}</p>
                    <ul>{string.Join("", labTests.Select(x => $"<li>{x.LabTest.TestName} - ₹{x.LabTest.Cost}</li>"))}</ul>
                    <p>Please complete the full payment to proceed with the tests.</p>
                    <p>Until then, your lab tests remain on hold.</p>
                    <br/>
                    <p>Regards,<br/>Hospital Management System</p>";
                }

                await _emailService.SendEmailAsync(patient.Email, subject, body);

                return cumulativePaid >= totalCost
                    ? "Full payment received. Lab tests scheduled."
                    : $"₹{dto.PaidAmount} received. ₹{remainingAmount} remaining.";
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while processing lab test payment.", ex);
            }
        }

        public async Task<PatientLabTestResultsListDto> GetCompletedLabResultsAsync(string patientId)
        {
            try
            {
                if (string.IsNullOrEmpty(patientId))
                    throw new ArgumentException("Invalid Patient Id.");
                var baseUrl = _httpContextAccessor.HttpContext?.Request.Scheme + "://" + _httpContextAccessor.HttpContext?.Request.Host;
                // 🔍 Step 1: Fetch patient + completed lab tests
                var labTests = await _hMSDbContext.PatientLabTest_Tbl
                    .Include(x => x.LabTest)
                    .Include(x => x.Doctor)
                    .Include(x => x.Patient)
                    .Where(x => x.PatientId == patientId && x.Status == LabTestStatus.Completed)
                    .OrderByDescending(x => x.TestDate)
                    .ToListAsync();

                if (!labTests.Any())
                    throw new Exception("No completed lab tests found for this patient.");

                var patientName = labTests.First().Patient.Name;

                // 🔄 Step 2: Convert each to DTO
                var resultList = labTests.Select(x => new PatientLabTestResultDto
                {
                    LabTestId = x.LabTest.LabTestId,
                    TestName = x.LabTest.TestName,
                    TestDate = x.TestDate,
                    Result = x.Result ?? "No result found",
                    ReportFileUrl = string.IsNullOrEmpty(x.ReportFilePath) ? null : $"{baseUrl}/{x.ReportFilePath}",
                    DoctorName = x.Doctor?.Name ?? "Unknown",
                    Status = x.Status.ToString()
                }).ToList();

                // 🔚 Step 3: Return final list
                return new PatientLabTestResultsListDto
                {
                    PatientId = patientId,
                    PatientName = patientName,
                    Results = resultList
                };
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while retrieving completed lab results.", ex);
            }
        }

        public async Task<FileDto> DownloadLabReportAsync(Guid labTestId, string userId, List<string> roles)
        {
            try
            {

                // 🔍 Fetch the lab test
                var labTest = await _hMSDbContext.PatientLabTest_Tbl
                    .Include(x => x.LabTest)
                    .FirstOrDefaultAsync(x => x.LabTest.LabTestId == labTestId);

                if (labTest == null)
                    throw new Exception("Lab test not found.");

                // 🔐 Ownership check (keep roles logic unchanged)
                if (roles.Contains("Patient") && labTest.PatientId != userId)
                    throw new UnauthorizedAccessException("This report does not belong to you.");

                if (roles.Contains("Doctor") && labTest.DoctorId != userId)
                    throw new UnauthorizedAccessException("You did not prescribe this test.");

                if (!roles.Contains("Patient") && !roles.Contains("Doctor"))
                    throw new UnauthorizedAccessException("Your role is not allowed.");

                // 📁 File path validation
                if (string.IsNullOrEmpty(labTest.ReportFilePath))
                    throw new Exception("No report file uploaded for this test.");

                // ✅ Correct file path (assuming ReportFilePath = "LabReports/xyz.pdf")
                var fullPath = Path.Combine("wwwroot", labTest.ReportFilePath);

                if (!System.IO.File.Exists(fullPath))
                {
                    Console.WriteLine($"[DEBUG] File not found at: {fullPath}"); // Optional debug log
                    throw new FileNotFoundException("Report file not found on server.");
                }

                // 📦 Prepare FileDto
                var fileBytes = await File.ReadAllBytesAsync(fullPath);
                var fileName = $"{labTest.LabTest.TestName}_Report.pdf";

                return new FileDto
                {
                    FileBytes = fileBytes,
                    FileName = fileName,
                    ContentType = "application/pdf"
                };
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while downloading the lab report.", ex);
            }
        }

        public async Task<PrescriptionDto> GetPrescriptionByIdAsync(string loggedInPatientId, Guid prescriptionId)
        {
            try
            {
                if (prescriptionId == Guid.Empty)
                    throw new ArgumentException("Prescription ID is required.");

                // 🔍 Validate Logged-In Patient ID
                if (string.IsNullOrEmpty(loggedInPatientId))
                    throw new ArgumentException("Patient ID is missing or user not authenticated.");

                // Step 2: Get the prescription with its details
                var prescription = await _hMSDbContext.Prescription_Tbl
                     .Include(p => p.Appointment)
                         .ThenInclude(a => a.Doctor)
                     .Include(p => p.Details)
                     .FirstOrDefaultAsync(p =>
                         p.PrescriptionId == prescriptionId &&
                         p.Appointment.PatientId == loggedInPatientId &&     // ✅ Ensure access control
                         p.Appointment.Status == AppointmentStatus.Completed
                     );
                // Step 3: Handle not found
                if (prescription == null)
                    throw new Exception("Prescription not found.");

                var prescriptionDto = new PrescriptionDto
                {
                    PrescriptionId = prescription.PrescriptionId,
                    Note = prescription.Notes ?? string.Empty,
                    DoctorName = prescription.Appointment.Doctor?.Name ?? "N/A",
                    AppointmentDate = prescription.Appointment.AppointmentDate,
                    Details = prescription.Details.Select(d => new PrescriptionDetailsDto
                    {
                        Medicine = d.MedicineName,
                        Dosage = d.Dosage,
                        Frequency = d.Frequency,
                        Duration = d.Duration,
                        Instructions = d.Instructions
                    }).ToList()
                };

                return prescriptionDto;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while retrieving the prescription by ID.", ex);
            }
        }
    }
}
