using HospitalManagementSystem.Bl.AppRepo.LabTechnician.IService;
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

namespace HospitalManagementSystem.Bl.AppRepo.LabTechnician.Implementation
{
    public class LabTechnicianService : ILabTechnicianService
    {
        private readonly HMSDbContext _hMSDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        public LabTechnicianService(HMSDbContext hMSDbContext, UserManager<ApplicationUser> userManager, IEmailService emailService)
        {
            _hMSDbContext = hMSDbContext;
        }

        public async Task<List<PatientLabTestGroupDto>> GetReadyLabTestsAsync()
        {
            try
            {
                // ✅ Fetch only Paid lab tests with required relations
                var readyTests = await _hMSDbContext.PatientLabTest_Tbl
                    .Include(x => x.Patient)
                    .Include(x => x.Doctor)
                    .Include(x => x.LabTest)
                    .Include(x => x.Appointment)
                    .Where(x => x.Status == LabTestStatus.Completed)
                    .OrderBy(x => x.Appointment.BookingDate)      // 1️⃣ First-come-first-served
                    .ThenBy(x => x.PatientId)                     // 2️⃣ Group by patient
                    .ThenBy(x => x.CreatedAt)                     // 4️⃣ Oldest prescribed first
                    .ToListAsync();

                if (readyTests == null || !readyTests.Any())
                {
                    throw new Exception("No ready lab tests found.");
                }

                // ✅ Group by Patient
                var grouped = readyTests
                .GroupBy(x => new { x.PatientId, x.Patient.Name })
                .Select(group => new PatientLabTestGroupDto
                {
                    PatientId = group.Key.PatientId,
                    PatientName = group.Key.Name,
                    LabTests = group.Select(x => new LabTestItemDto
                    {
                        LabTestId = x.LabTest.LabTestId,
                        TestName = x.LabTest.TestName,
                        Status = x.Status.ToString(),
                        TestDate = x.TestDate,
                        Notes = x.Notes,
                        DoctorName = x.Doctor.Name,
                        AppointmentDate = x.Appointment.AppointmentDate
                    }).ToList()
                }).ToList();

                return grouped;
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while fetching ready lab tests.", ex);
            }
        }

        public async Task<LabTestStartResponseDto> StartLabTestAsync(Guid labTestId)
        {
            try
            {

                var test = await _hMSDbContext.PatientLabTest_Tbl
                    .Include(x => x.Patient)
                    .Include(x => x.Doctor)
                    .Include(x => x.LabTest)
                    .FirstOrDefaultAsync(x => x.LabTestId == labTestId);

                if (test == null)
                    throw new Exception("Lab test not found.");
                if (test.Status == LabTestStatus.Completed)
                    throw new Exception("Lab test has already been completed.");
                if (test.Status == LabTestStatus.InProgress)
                {
                    throw new Exception("Lab test is already in progress.");
                }

                if (test.Status != LabTestStatus.Completed)
                    throw new Exception("Lab test is not ready to be started.");

                // ✅ Update status
                test.Status = LabTestStatus.InProgress;
                test.UpdatedAt = DateTime.UtcNow;

                await _hMSDbContext.SaveChangesAsync();

                return new LabTestStartResponseDto
                {
                    LabTestId = test.LabTestId,
                    TestName = test.LabTest.TestName,
                    PatientName = test.Patient.Name,
                    DoctorName = test.Doctor.Name,
                    StartedAt = test.UpdatedAt.Value,
                    Status = test.Status.ToString()
                };
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while starting the lab test.", ex);
            }
        }

        public async Task<LabTestResultResponseDto> SubmitLabTestResultAsync(Guid labTestId, LabTestResultUploadDto dto)
        {
            using var transaction = await _hMSDbContext.Database.BeginTransactionAsync();
            try
            {
                // 🔹 Step 1: LabTest record dhoondo
                var test = await _hMSDbContext.PatientLabTest_Tbl
                    .Include(x => x.Patient)
                    .Include(x => x.Doctor)
                    .Include(x => x.LabTest)
                    .FirstOrDefaultAsync(x => x.LabTest.LabTestId == labTestId);

                if (test == null)
                    throw new Exception("Lab test not found.");

                // 🔹 Step 2: Check karo ki test InProgress hona chahiye
                if (test.Status != LabTestStatus.InProgress)
                    throw new Exception("Only tests in progress can be completed.");

                // 🔹 Step 3: Result text empty nahi hona chahiye
                if (string.IsNullOrWhiteSpace(dto.Result))
                    throw new ArgumentException("Result cannot be empty.");

                // ✅ Set result
                test.Result = dto.Result;

                // 🔹 Step 4: Agar file hai to save karo
                if (dto.ReportFile != null && dto.ReportFile.Length > 0)
                {
                    // wwwroot folder create karo agar nahi hai to
                    var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    Directory.CreateDirectory(wwwRootPath);

                    // wwwroot/LabReports bhi create karo
                    var reportFolder = Path.Combine(wwwRootPath, "LabReports");
                    Directory.CreateDirectory(reportFolder);

                    // FileName generate karo
                    var fileName = $"LabReport_{labTestId}_{DateTime.UtcNow.Ticks}{Path.GetExtension(dto.ReportFile.FileName)}";
                    var fullPath = Path.Combine(reportFolder, fileName);

                    // File save karo
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await dto.ReportFile.CopyToAsync(stream);
                    }

                    // File path set karo DB ke liye
                    test.ReportFilePath = Path.Combine("LabReports", fileName).Replace("\\", "/");
                }

                // 🔹 Step 5: Status change karo Completed par
                test.Status = LabTestStatus.Completed;
                test.UpdatedAt = DateTime.UtcNow;

                await _hMSDbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                // 🔹 Step 6: Prepare response
                return new LabTestResultResponseDto
                {
                    LabTestId = test.LabTestId,
                    TestName = test.LabTest.TestName,
                    PatientName = test.Patient.Name,
                    DoctorName = test.Doctor.Name,
                    Status = test.Status.ToString(),
                    ReportFilePath = test.ReportFilePath,
                    UpdatedAt = test.UpdatedAt.Value
                };
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                throw new Exception("An error occurred while submitting the lab test result.", ex);
            }
        }
    }
}
