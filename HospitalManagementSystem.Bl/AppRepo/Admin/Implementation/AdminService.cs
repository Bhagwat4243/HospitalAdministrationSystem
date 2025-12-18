using AutoMapper;
using HospitalManagementSystem.Bl.AppRepo.Admin.IService;
using HospitalManagementSystem.Bl.AuthRepo.IService;
using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Db.Model.AuthModel;
using HospitalManagementSystem.Dto.AppDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.AppRepo.Admin.Implementation
{
    public class AdminService : IAdminService
    {
        private readonly HMSDbContext _hMSDbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        public AdminService(HMSDbContext hMSDbContext,IMapper mapper, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IEmailService _emailService)
        {
            _hMSDbContext = hMSDbContext;
            _emailService = _emailService;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<List<ChemistDetailsDto>> GetAllChemist()
        {
            try
            {
                var chemists = await _hMSDbContext.Chemist_Tbl
                   .Include(p => p.AppUser)
                   .ToListAsync();

                if (chemists == null || !chemists.Any())
                {
                    throw new Exception("No chemist found.");
                }
                var allChemistDtos = new List<ChemistDetailsDto>();
                foreach (var chemist in chemists)
                {
                    var chemistDto = new ChemistDetailsDto
                    {
                        ChemistId = chemist.ChemistId,
                        Name = chemist.AppUser.Name,
                        Email = chemist.AppUser.Email,
                        Gender = chemist.AppUser.Gender,
                        UserId = chemist.AppUser.Id,
                        AadharNo = chemist.AppUser.AddharNo,
                        State = chemist.AppUser.State,
                        City = chemist.AppUser.City,
                        DateOfBirth = chemist.AppUser.DOB,
                        Pincode = chemist.AppUser.PostalCode,
                        PhoneNumber = chemist.AppUser.PhoneNumber,
                        LicenseNumber = chemist.LicenseNumber,
                        LicenseExpiryDate = chemist.LicenseExpiryDate,
                        LicenseIssuedBy = chemist.LicenseIssuedBy,
                        Qualification = chemist.Qualification,
                        ExperienceInYears = chemist.ExperienceInYears
                    };
                    allChemistDtos.Add(chemistDto);
                }
                return allChemistDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<DoctorDetailsDto>> GetAllDoctor()
        {
            try
            {
                var doctors = await _hMSDbContext.Doctor_Tbl
                   .Include(d => d.AppUser)  
                   .ToListAsync();

                if (doctors == null || !doctors.Any())
                {
                    return new List<DoctorDetailsDto>(); 
                }
                var result = doctors.Select(doctor => new DoctorDetailsDto
                {
                    Name = doctor.AppUser?.Name ?? string.Empty,  
                    Email = doctor.AppUser?.Email ?? string.Empty,
                    Specialization = doctor.Specialization,
                    ExperienceYears = doctor.ExperienceYears,
                    Qualifications = !string.IsNullOrEmpty(doctor.Qualifications)
                        ? doctor.Qualifications.Split(',').ToList()
                        : new List<string>(),
                    AlternatePhoneNo = doctor.AlternatePhoneNo ?? string.Empty,
                    LicenseNumber = doctor.LicenseNumber,
                    LicenseExpiryDate = doctor.LicenseExpiryDate,
                    AvailableDays = !string.IsNullOrEmpty(doctor.AvailableDays)
                        ? doctor.AvailableDays.Split(',').ToList()
                        : new List<string>(),
                    StartTime = doctor.StartTime,
                    EndTime = doctor.EndTime,
                    RoomNumber = doctor.RoomNumber
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<LabTechnicianDetailsDto>> GetAllLabTechnician()
        {
            try
            {
                var labtechnicians =await _hMSDbContext.LabTechnician_Tbl
                    .Include(l => l.AppUser)
                    .ToListAsync();
                if (labtechnicians == null || !labtechnicians.Any())
                {
                    throw new Exception("No lab technicians found.");
                }
                var allLabTechnicianDtos = new List<LabTechnicianDetailsDto>();
                foreach(var l in labtechnicians)
                {
                    var labtechniciandto = new LabTechnicianDetailsDto
                    {
                        UserId = l.AppUser.Id,
                        Name = l.AppUser.Name,
                        Gender = l.AppUser.Gender,
                        Email = l.AppUser.Email,
                        AadharNo = l.AppUser.AddharNo,
                        State = l.AppUser.State,
                        City = l.AppUser.City,
                        DateOfBirth = l.AppUser.DOB,
                        Pincode = l.AppUser.PostalCode,
                        PhoneNumber = l.AppUser.PhoneNumber,
                        Qualification = l.Qualification,
                        ExperienceInYears = l.ExperienceInYears,
                        Specialization = l.Specialization,
                        LicenseNumber = l.LicenseNumber,
                        LicenseExpiryDate = l.LicenseExpiryDate,
                        LicenseIssuedBy = l.LicenseIssuedBy   
                    };
                    allLabTechnicianDtos.Add(labtechniciandto);
                }
                return allLabTechnicianDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<NurseDetailsDto>> GetAllNurse()
        {
            try
            {
                var nurses = await _hMSDbContext.Nurse_Tbl
                    .Include(n => n.AppUser)
                    .ToListAsync();
                if (nurses == null || !nurses.Any())
                {
                    throw new Exception("No nurses found.");
                }
                var allNurseDtos = new List<NurseDetailsDto>();
                foreach (var n in nurses)
                {
                    var nursedtos = new NurseDetailsDto
                    {
                        NurseId = n.NurseId,
                        Name = n.AppUser.Name,
                        AadharNo = n.AppUser.AddharNo,
                        Email = n.AppUser.Email,
                        State = n.AppUser.State,
                        City = n.AppUser.City,
                        Gender = n.AppUser.Gender,
                        DateOfBirth = n.AppUser.DOB,
                        Pincode = n.AppUser.PostalCode,
                        PhoneNumber = n.AppUser.PhoneNumber,
                        Qualification = n.Qualification,
                        RegistrationCouncil = n.RegistrationCouncil,
                        CouncilRegistrationNumber = n.CouncilRegistrationNumber,
                        ExperienceInYears = n.ExperienceInYears,
                        Departments = n.Departments,
                        UserId = n.AppUser.Id
                    };
                    allNurseDtos.Add(nursedtos);
                }
                return allNurseDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<PatientDetailsDto>> GetAllPatient()
        {
            try
            {
                var patients = await _hMSDbContext.Patient_Tbl
                    .Include(p => p.AppUser)
                    .ToListAsync();

                if (patients == null || !patients.Any())
                {
                    throw new Exception("No patients found.");
                }
                var allPatientDtos = new List<PatientDetailsDto>();
                foreach (var patient in patients)
                {
                    var patientDtos = new PatientDetailsDto
                    {
                        Name = patient.AppUser.Name,
                        Email = patient.AppUser.Email,
                        AadharNo = patient.AppUser.AddharNo,
                        State = patient.AppUser.State,
                        City = patient.AppUser.City,
                        Gender = patient.AppUser.Gender,
                        DateOfBirth = patient.AppUser.DOB,
                        Pincode = patient.AppUser.PostalCode,
                        PhoneNumber = patient.AppUser.PhoneNumber,
                        BloodGroup = patient.BloodGroup,
                        ChronicDiseases = patient.ChronicDiseases,
                        MedicalHistory = patient.MedicalHistory,
                        RegistrationDate = patient.RegistrationDate,


                    };
                    allPatientDtos.Add(patientDtos);
                }


                return allPatientDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<AppointmentDto>> GetAllAppointment()
        {
            try
            {
                var appointments = await _hMSDbContext.Appointment_Tbl.ToListAsync();
                if (appointments == null || !appointments.Any())
                {
                    throw new Exception("No appointments found.");
                }
                var appointmentDtos = new List<AppointmentDto>();
                foreach (var appointment in appointments)
                {
                    var appointmentDto = new AppointmentDto
                    {
                        AppointmentId = appointment.AppointmentId,
                        PatientId = appointment.PatientId,
                        DoctorId = appointment.DoctorId,
                        AppointmentDate = appointment.AppointmentDate,
                        Remark = appointment.Remark,
                        AppointmentTime = appointment.AppointmentDate, // Assuming AppointmentDate contains time
                        PatientName = appointment.Patient?.Name ?? "Unknown Patient", // Assuming Patient navigation property exists
                        DoctorName = appointment.Doctor?.Name ?? "Unknown Doctor", // Assuming Doctor navigation property exists
                        Status = appointment.Status
                    };
                    appointmentDtos.Add(appointmentDto);
                }
                return appointmentDtos;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LabTestDto> CreateLabTest(LabTestDto labTestDto)
        {
            try
            {
                if (labTestDto == null || string.IsNullOrEmpty(labTestDto.TestName) || labTestDto.Cost <= 0)
                {
                    throw new ArgumentException("Invalid lab test details.");
                }
                var labTest = new LabTest
                {
                    TestName = labTestDto.TestName,
                    SampleRequired = labTestDto.SampleRequired ?? "Not specified", // Default value if not provided
                    Duration = labTestDto.Duration ?? "Not specified", // Default value if not provided
                    IsActive = true, // Default to active
                    Preparation = labTestDto.Preparation ?? "Not specified", // Default value if not provided
                    UpdatedAt = null, // Initially null
                    Cost = labTestDto.Cost,
                    Description = labTestDto.Description,
                    CreatedAt = DateTime.UtcNow
                };
                _hMSDbContext.LabTest_Tbl.Add(labTest);
                await _hMSDbContext.SaveChangesAsync();
                return new LabTestDto
                {
                    TestName = labTest.TestName,
                    Cost = labTest.Cost,
                    Description = labTest.Description,
                    SampleRequired = labTest.SampleRequired,
                    Preparation = labTest.Preparation,
                    Duration = labTest.Duration    
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<LabTestDto>> GetAllLabTests()
        {
            try
            {
                var allLabTests = await _hMSDbContext.LabTest_Tbl.ToListAsync();
                if (allLabTests == null || !allLabTests.Any())
                {
                    throw new Exception("No lab tests found.");
                }
                var labTestDtos = _mapper.Map<List<LabTestDto>>(allLabTests);
                return labTestDtos;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<string> AddNewLabTest(LabTestDto labTestDto)
        {
            try
            {
                if (labTestDto == null || string.IsNullOrWhiteSpace(labTestDto.TestName))
                {
                    throw new ArgumentNullException(nameof(labTestDto), "Lab test details cannot be null or empty.");
                }

                // Check for duplicate test name (case-insensitive)
                var isDuplicate = await _hMSDbContext.LabTest_Tbl
                    .AnyAsync(t => t.TestName.ToLower() == labTestDto.TestName.ToLower());

                if (isDuplicate)
                {
                    throw new InvalidOperationException("A lab test with the same name already exists.");
                }

                // Create entity and map
                var newTest = _mapper.Map<LabTest>(labTestDto);
                newTest.UpdatedAt = null;
                newTest.CreatedAt = DateTime.Now; // Set the creation date to now
                newTest.IsActive = true;

                await _hMSDbContext.LabTest_Tbl.AddAsync(newTest);
                await _hMSDbContext.SaveChangesAsync();

                return $"Lab test added successfully.";
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<List<AppointmentResponseDto>> GetPendingAppointmentsAsync()
        {
            try
            {
                var pendingAppointments = await _hMSDbContext.Appointment_Tbl
                 .Where(a => a.Status == AppointmentStatus.Pending)
                 .Include(a => a.Doctor)
                 .Include(a => a.Patient)
                 .Select(a => new AppointmentResponseDto
                 {
                     AppointmentId = a.AppointmentId,
                     DoctorId = a.DoctorId,
                     DoctorName = a.Doctor.Name,
                     PatientId = a.PatientId,
                     PatientName = a.Patient.Name,
                     PatientGender = a.Patient.Gender,
                     PatientAge = a.Patient.Age,
                     AppointmentDate = a.AppointmentDate,
                     Remark = a.Remark,
                     Status = a.Status.ToString(),
                     BookingDate = a.BookingDate
                 })
                 .ToListAsync();

                return pendingAppointments;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<string> ConfirmAppointmentByAdminAsync(Guid appointmentId)
        {

            using var transaction = await _hMSDbContext.Database.BeginTransactionAsync();
            try
            {
                var appointment = await _hMSDbContext.Appointment_Tbl.Include(d => d.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                if (appointment == null)
                    throw new Exception("Appointment not found.");

                if (appointment.Status != AppointmentStatus.Pending)
                    throw new Exception("Only pending appointments can be confirmed.");

                var consultantFee = await _hMSDbContext.Doctor_Tbl
                    .Where(d => d.UserId == appointment.DoctorId)
                    .Select(d => d.ConsultantFee)
                    .FirstOrDefaultAsync();

                appointment.Status = AppointmentStatus.Confirmed;
                appointment.UpdatedAt = DateTime.Now;

                _hMSDbContext.Appointment_Tbl.Update(appointment);
                await _hMSDbContext.SaveChangesAsync();

                // Send email to patient
                var patient = await _userManager.FindByIdAsync(appointment.PatientId);
                if (patient == null)
                {
                    throw new Exception("Patient not found for the appointment.");
                }
                string body = $@"
                <p>Dear <strong>{patient.Name}</strong>,</p>

                <p>Your appointment request has been <strong style='color:green;'>Confirmed</strong> successfully.</p>

                <p><strong>Appointment Details:</strong></p>
                   <ul>
                       <li><strong>Doctor ID:</strong> {appointment.DoctorId}</li>
                       <li><strong>Doctor Name:</strong> {appointment.Doctor.Name}</li>
                       <li><strong>Consultant Fee<strong> {consultantFee}</li>
                   </ul>

                <p style='margin-top:10px;'>To proceed your appointment, please complete the payment as soon as possible.The consultation with your doctor will only be valid after the payment is successfully made.</p>

                <p><strong>Note:</strong> You will receive a confirmation email and access to your appointment details once your payment is verified/fully paid.</p>

                 <br />
                <p>Thank you for choosing <strong>Hospital Management System</strong>.</p>
                <p>We wish you good health!</p>
                ";

                await _emailService.SendEmailAsync(patient.Email, "Appointment Confirmed", body);

                await transaction.CommitAsync();

                return "Appointment confirmed successfully.";
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<string> CancelAppointmentByAdminAsync(Guid appointmentId, string? cancelReason = null)
        {
           
                using var transaction = await _hMSDbContext.Database.BeginTransactionAsync();
                try
                {
                    var appointment = await _hMSDbContext.Appointment_Tbl.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

                    if (appointment == null)
                        throw new Exception("Appointment not found.");

                    if (appointment.Status != AppointmentStatus.Pending)
                        throw new Exception("Only pending appointments can be cancelled.");

                    appointment.Status = AppointmentStatus.Cancelled;
                    appointment.UpdatedAt = DateTime.Now;

                    _hMSDbContext.Appointment_Tbl.Update(appointment);
                    await _hMSDbContext.SaveChangesAsync();

                    // Send email to patient
                    var patient = await _userManager.FindByIdAsync(appointment.PatientId);
                    if (patient == null)
                    {
                        throw new ArgumentNullException($"Patient with ID : {appointment.PatientId} not found for the appointment.");
                    }
                    string reasonText = !string.IsNullOrEmpty(cancelReason) ? $"<br>Reason : {cancelReason}" : "";
                    string body = $"Dear {patient.Name},<br>Your appointment on {appointment.AppointmentDate} has been <strong>Cancelled<strong> {reasonText}";
                    await _emailService.SendEmailAsync(patient.Email, "Appointment Cancelled", body);
                    await transaction.CommitAsync();

                    return "Appointment cancelled successfully.";
                }
                catch (Exception ex)
                { 
                    throw new Exception();
                }

        }
    }
}
