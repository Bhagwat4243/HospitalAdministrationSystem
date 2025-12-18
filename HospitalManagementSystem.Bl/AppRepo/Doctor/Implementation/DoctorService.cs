using AutoMapper;
using HospitalManagementSystem.Bl.AppRepo.Doctor.IService;
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

namespace HospitalManagementSystem.Bl.AppRepo.Doctor.Implementation
{
    public class DoctorService : IDoctorService
    {
        private readonly HMSDbContext _hMSDbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public DoctorService(HMSDbContext hMSDbContext, IMapper mapper, UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _hMSDbContext = hMSDbContext;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;

        }
        public async Task<string> PrescribeLabTestsAsync(PatientLabTestDto prescribeLabTestDto, string doctorId)
        {
            try
            {
                if (prescribeLabTestDto == null)
                    throw new ArgumentException("Invalid lab test data.");

                if (!prescribeLabTestDto.LabTestIds.Any())
                    throw new ArgumentException("No lab tests provided.");


                // ✅ Validate confirmed appointment between doctor and patient
                var appointment = await _hMSDbContext.Appointment_Tbl
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == prescribeLabTestDto.AppointmentId
                                              && a.Status == AppointmentStatus.Confirmed
                                              && a.DoctorId == doctorId);


                if (appointment == null)
                    throw new ArgumentException("No confirmed appointment found for this patient with the specified doctor.");

                if (appointment.DoctorId != doctorId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to prescribe Lab tests for this patient.");
                }

                // ✅ Fetch valid LabTests
                var labTests = await _hMSDbContext.LabTest_Tbl
                    .Where(t => prescribeLabTestDto.LabTestIds.Contains(t.LabTestId))
                    .ToListAsync();

                if (labTests.Count != prescribeLabTestDto.LabTestIds.Count)
                    throw new Exception("One or more lab tests are invalid.");

                // ✅ Create PatientLabTests
                var patientLabTests = labTests.Select(test => new PatientLabTest
                {
                    AppointmentId = appointment.AppointmentId,        // 🔴 Link to Appointment
                    PatientId = appointment.Patient.Id,         // 🔴 Still storing for easy filtering
                    DoctorId = doctorId,                               // 🔴 Still storing for audit/tracking
                    LabTestId = test.LabTestId,
                    TestDate = prescribeLabTestDto.TestDate,
                    Notes = prescribeLabTestDto.Notes,
                    Result = null,
                    ReportFilePath = null,
                    Status = LabTestStatus.Requested,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                }).ToList();

                await _hMSDbContext.PatientLabTest_Tbl.AddRangeAsync(patientLabTests);
                await _hMSDbContext.SaveChangesAsync();

               

                // ✅ Prepare response DTO
                //var response = new PrescribeLabTestResponseDto
                //{
                //    PatientId = appointment.Patient.Id,
                //    PatientName = appointment.Patient.Name,
                //    OrderedById = doctorId,
                //    DoctorName = appointment.Doctor.Name,
                //    AppointmentId = appointment.AppointmentId,
                //    TestDate = prescribeLabTestDto.TestDate,
                //    LabTests = labTests.Select(test => new PrescribedLabTestItemDto
                //    {
                //        LabTestId = test.LabTestId,
                //        TestName = test.TestName,
                //        Cost = test.Cost
                //    }).ToList()
                //};

                return "Prescription Added Successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Error while prescribing lab tests: " + ex.Message);
            }
        }

        public async Task<PrescriptionDto> CreatePrescription(PrescriptionDto prescriptionDto)
        {
            try
            {
                if (prescriptionDto == null || prescriptionDto.Details == null || !prescriptionDto.Details.Any())
                {
                    throw new ArgumentException("Prescription details cannot be null or empty.");
                }
                using var transaction = await _hMSDbContext.Database.BeginTransactionAsync();
                var prescription = new Prescriptions
                {
                    AppointmentId = prescriptionDto.AppointmentId,
                    Notes = prescriptionDto.Note,
                    PrescriptionDate = DateTime.UtcNow
                };
                _hMSDbContext.Prescription_Tbl.Add(prescription);
                await _hMSDbContext.SaveChangesAsync();
                foreach (var detail in prescriptionDto.Details)
                {
                    var prescriptionDetail = new PrescriptionDetails
                    {
                        PrescriptionId = prescription.PrescriptionId,
                        MedicineName = detail.Medicine,
                        Dosage = detail.Dosage,
                        Frequency = detail.Frequency,
                        Duration = detail.Duration
                    };
                    _hMSDbContext.PrescriptionDetail_Tbl.Add(prescriptionDetail);
                }
                await _hMSDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                return new PrescriptionDto
                {
                    AppointmentId = prescription.AppointmentId,
                    Note = prescription.Notes,
                    Details = prescriptionDto.Details.Select(d => new PrescriptionDetailsDto
                    {
                        Medicine = d.Medicine,
                        Dosage = d.Dosage,
                        Frequency = d.Frequency,
                        Duration = d.Duration
                    }).ToList()
                };

            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("An error occurred while creating the prescription.", ex);
            }
        }

        public async Task<List<PatientDetailsDto>> GetAllPatientsAssignedToDoctor(string doctorId)
        {
            try
            {
                var appointments = await _hMSDbContext.Appointment_Tbl
        .        Where(a => a.DoctorId == doctorId)
                 .Select(a => a.PatientId)
                   .Distinct()
                    .ToListAsync();

                if (!appointments.Any())
                {
                    return new List<PatientDetailsDto>();
                }

                // Get user & patient data
                var patients = await (from appUser in _hMSDbContext.Users
                                      join patient in _hMSDbContext.Patient_Tbl
                                      on appUser.Id equals patient.UserId
                                      where appointments.Contains(appUser.Id)
                                      select new PatientDetailsDto
                                      {
                                          UserId = appUser.Id,
                                          Name = appUser.Name,
                                          Email = appUser.Email,
                                          PhoneNumber = appUser.PhoneNumber,
                                          Gender = appUser.Gender,
                                          AadharNo = appUser.AddharNo,
                                          DateOfBirth = appUser.DOB,
                                          State = appUser.State,
                                          City = appUser.City,
                                          Pincode = appUser.PostalCode,
                                          BloodGroup = patient.BloodGroup,
                                          ChronicDiseases = patient.ChronicDiseases,
                                          MedicalHistory = patient.MedicalHistory,
                                          RegistrationDate = patient.RegistrationDate
                                      }).ToListAsync();

                return patients;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("An error occurred while retrieving patients assigned to the doctor.", ex);
            }
        }

        public async Task<List<LabTestDto>> GetLabTests()
        {
            try
            {
                var labTests = await _hMSDbContext.LabTest_Tbl
                    .Select(l => new LabTestDto
                    {
                        TestName = l.TestName,
                        Description = l.Description,
                        SampleRequired = l.SampleRequired,
                        Preparation = l.Preparation,
                        Cost = l.Cost,
                        Duration = l.Duration
                    }).ToListAsync();
                if(labTests == null)
                {
                    throw new ArgumentException("No lab tests found.");
                }
                return labTests;
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("An error occurred while retrieving lab tests.", ex);
            }
        }

        public async Task<PatientDetailsDto> GetPatientDetailsById(string patientId)
        {
            try
            {
                var appointmentExists = await _hMSDbContext.Appointment_Tbl
                  .AnyAsync(a => a.PatientId == patientId);

                if (!appointmentExists)
                {
                    throw new ArgumentException("No patient found with the provided ID.");
                }

                // Join ApplicationUser and Patient table to get full details
                var patientDetails = await (from appUser in _hMSDbContext.Users
                                            join patient in _hMSDbContext.Patient_Tbl
                                            on appUser.Id equals patient.UserId
                                            where appUser.Id == patientId
                                            select new PatientDetailsDto
                                            {
                                                UserId = appUser.Id,
                                                Name = appUser.Name,
                                                Email = appUser.Email,
                                                PhoneNumber = appUser.PhoneNumber,
                                                Gender = appUser.Gender,
                                                AadharNo = appUser.AddharNo,
                                                DateOfBirth = appUser.DOB,
                                                State = appUser.State,
                                                City = appUser.City,
                                                Pincode = appUser.PostalCode,
                                                BloodGroup = patient.BloodGroup,
                                                ChronicDiseases = patient.ChronicDiseases,
                                                MedicalHistory = patient.MedicalHistory,
                                                RegistrationDate = patient.RegistrationDate
                                            }).FirstOrDefaultAsync();

                if (patientDetails == null)
                {
                    throw new ArgumentException("Patient details not found.");
                }
                return patientDetails;
            } 
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("An error occurred while retrieving patient details.", ex);
            }
        }

        public async Task<PrescriptionDto> GetPatientPrescription(string patientId)
        {
            try
            {
                var prescription = await _hMSDbContext.Prescription_Tbl
                    .Include(p => p.Details)
                    .FirstOrDefaultAsync(p => p.Appointment.PatientId == patientId);
                if (prescription == null)
                {
                    throw new ArgumentException("No prescription found for the provided patient ID.");
                }
                return new PrescriptionDto
                {
                    AppointmentId = prescription.AppointmentId,
                    Note = prescription.Notes,
                    Details = prescription.Details.Select(d => new PrescriptionDetailsDto
                    {
                        Medicine = d.MedicineName,
                        Dosage = d.Dosage,
                        Frequency = d.Frequency,
                        Duration = d.Duration
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<PrescriptionDto> UpdatePrescription(PrescriptionDto prescriptionDto, Guid PrescriptionId)
        {
            try
            {
                if (prescriptionDto == null || prescriptionDto.Details == null || !prescriptionDto.Details.Any())
                {
                    throw new ArgumentException("Prescription details cannot be null or empty.");
                }
                var existingPrescription = await _hMSDbContext.Prescription_Tbl
                    .Include(p => p.Details)
                    .FirstOrDefaultAsync(p => p.PrescriptionId == PrescriptionId);
                if (existingPrescription == null)
                {
                    throw new ArgumentException("Prescription not found.");
                }
                existingPrescription.Notes = prescriptionDto.Note;
                existingPrescription.PrescriptionDate = DateTime.UtcNow;
                // Clear existing details
                _hMSDbContext.PrescriptionDetail_Tbl.RemoveRange(existingPrescription.Details);
                // Add new details
                foreach (var detail in prescriptionDto.Details)
                {
                    var prescriptionDetail = new PrescriptionDetails
                    {
                        PrescriptionId = existingPrescription.PrescriptionId,
                        MedicineName = detail.Medicine,
                        Dosage = detail.Dosage,
                        Frequency = detail.Frequency,
                        Duration = detail.Duration
                    };
                    _hMSDbContext.PrescriptionDetail_Tbl.Add(prescriptionDetail);
                }
                await _hMSDbContext.SaveChangesAsync();
                return new PrescriptionDto
                {
                    AppointmentId = existingPrescription.AppointmentId,
                    Note = existingPrescription.Notes,
                    Details = prescriptionDto.Details.Select(d => new PrescriptionDetailsDto
                    {
                        Medicine = d.Medicine,
                        Dosage = d.Dosage,
                        Frequency = d.Frequency,
                        Duration = d.Duration
                    }).ToList()
                };
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                throw new Exception("An error occurred while updating the prescription.", ex);
            }
        }

        public async Task<List<DoctorViewAppointmentDto>> GetDoctorAppointments(string doctorId)
        {
            try
            {
                var allAppointments = await (from a in _hMSDbContext.Appointment_Tbl
                                             join p in _hMSDbContext.AppointmentPayment_Tbl
                                                 on a.AppointmentId equals p.AppointmentId
                                             where a.DoctorId == doctorId
                                                   && a.Status == AppointmentStatus.Confirmed
                                                   && a.IsPaid == true
                                                   && p.PaymentStatus == PaymentStatus.Paid
                                             select new DoctorViewAppointmentDto
                                             {
                                                 AppointmentId = a.AppointmentId,
                                                 PatientName = a.Patient.Name,
                                                 PatientGender = a.Patient.Gender,
                                                 PatientAge = a.Patient.Age,
                                                 AppointmentDate = a.AppointmentDate,
                                                 TotalPaid = p.PaidAmount,
                                                 Status = a.Status.ToString()
                                             }).ToListAsync();

                return allAppointments;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving doctor appointments.", ex);
            }
        }

        public async Task<List<LabTestDetailDto>> GetAvailableLabTestsAsync()
        {
            try
            {
                // Step 1: Fetch only active lab tests
                var labTests = await _hMSDbContext.LabTest_Tbl
                    .Where(test => test.IsActive)
                    .ToListAsync();

                // Step 2: Handle case if no test found
                if (labTests == null || !labTests.Any())
                    return new List<LabTestDetailDto>();

                // Step 3: Map to DTO
                var testDtos = labTests.Select(test => new LabTestDetailDto
                {
                    LabTestId = test.LabTestId,
                    TestName = test.TestName,
                    SampleRequired = test.SampleRequired,
                    Preparation = test.Preparation,
                    Duration = test.Duration,
                    Cost = test.Cost
                }).ToList();

                return testDtos;
            }
            catch(Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<PrescriptionResponseDto> CreatePrescriptionWithDetailsAsync(PrescriptionDto createPrescriptionDto, string doctorId)
        {
            using var transaction = await _hMSDbContext.Database.BeginTransactionAsync();
            try
            {
                if (createPrescriptionDto == null || createPrescriptionDto.Details == null)
                {
                    throw new ArgumentException("Invalid prescription data provided.");
                }
                //bool isAppointmentIdExists = await _doctorDb.Appointment_tbl
                //              .AnyAsync(a => a.AppointmentId == createPrescriptionDto.AppointmentId);
                //if (isAppointmentIdExists == false)
                //{
                //    throw new ArgumentException($"Appointment ID {createPrescriptionDto.AppointmentId} does not exist.");
                //}
                var appointmentDetails = await _hMSDbContext.Appointment_Tbl
                    .Include(a => a.Patient)
                    .Include(a => a.Doctor)
                    .FirstOrDefaultAsync(a => a.AppointmentId == createPrescriptionDto.AppointmentId);

                if (!appointmentDetails.IsPaid)
                {
                    throw new InvalidOperationException("The appointment must be paid before creating a prescription.");
                }

                if (appointmentDetails == null)
                {
                    throw new ArgumentException($"Appointment ID {createPrescriptionDto.AppointmentId} does not exist.");
                }
                if (appointmentDetails.DoctorId != doctorId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to create a prescription for this appointment.");
                }
                var prescription = new Prescriptions
                {
                    AppointmentId = createPrescriptionDto.AppointmentId,
                    Notes = createPrescriptionDto.Note,
                    PrescriptionDate = DateTime.Now
                };
                await _hMSDbContext.Prescription_Tbl.AddAsync(prescription);
                await _hMSDbContext.SaveChangesAsync();

                appointmentDetails.Status = AppointmentStatus.Completed; // Assuming you want to mark it as completed after prescription
                appointmentDetails.UpdatedAt = DateTime.Now;
                await _hMSDbContext.SaveChangesAsync();

                var prescriptionDetails = createPrescriptionDto.Details.Select(m => new PrescriptionDetails
                {
                    PrescriptionId = prescription.PrescriptionId,
                    Dosage = m.Dosage,
                    Frequency = m.Frequency,
                    Duration = m.Duration,
                    MedicineName = m.Medicine
                }).ToList();
                await _hMSDbContext.PrescriptionDetail_Tbl.AddRangeAsync(prescriptionDetails);
                await _hMSDbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                var prescriptionResponse = _mapper.Map<PrescriptionResponseDto>(prescription);
                prescriptionResponse.DoctorName = appointmentDetails.Doctor.Name;
                prescriptionResponse.PatientName = appointmentDetails.Patient.Name;
                return prescriptionResponse;
            }
            catch( Exception ex)
            {
                throw new Exception();
            }
        }
    }
}
