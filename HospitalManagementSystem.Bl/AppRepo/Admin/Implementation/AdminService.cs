using HospitalManagementSystem.Bl.AppRepo.Admin.IService;
using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Dto.AppDto;
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
        public AdminService(HMSDbContext hMSDbContext)
        {
            _hMSDbContext = hMSDbContext;
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
    }
}
