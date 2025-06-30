using HospitalManagementSystem.Bl.AppRepo.Doctor.IService;
using HospitalManagementSystem.Dto.AppDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        public readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        [HttpPost("CreatePrescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreatePrescription([FromBody] PrescriptionDto prescriptionDto)
        {
            if (prescriptionDto == null)
            {
                return BadRequest("Prescription details cannot be null.");
            }
            try
            {
                var result = await _doctorService.CreatePrescription(prescriptionDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the prescription.");
            }
        }
        [HttpPost("PrescribeLabTestsAsync")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> PrescribeLabTestsAsync([FromBody] PatientLabTestDto patientLabTestDto)
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (patientLabTestDto == null)
            {
                return BadRequest("Patient lab test details cannot be null.");
            }
            try
            {
                var result = await _doctorService.PrescribeLabTestsAsync(patientLabTestDto, doctorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the patient lab test.");
            }
        }
        [HttpPut("UpdatePrescription/{prescriptionId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdatePrescription([FromBody] PrescriptionDto prescriptionDto, Guid prescriptionId)
        {
            if (prescriptionDto == null)
            {
                return BadRequest("Prescription details cannot be null.");
            }
            try
            {
                var result = await _doctorService.UpdatePrescription(prescriptionDto, prescriptionId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the prescription.");
            }
        }
        [HttpGet("GetAllPatientsAssignedToDoctor")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetAllPatientsAssignedToDoctor()
        {
            var doctorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(doctorId))
            {
                return BadRequest("Doctor ID cannot be null or empty.");
            }
            try
            {
                var result = await _doctorService.GetAllPatientsAssignedToDoctor(doctorId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving patients assigned to the doctor.");
            }
        }
        [HttpGet("GetPatientDetailsById")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientDetailsById(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                return BadRequest("Patient ID cannot be null or empty.");
            }
            try
            {
                var result = await _doctorService.GetPatientDetailsById(patientId);
                if (result == null)
                {
                    return NotFound("Patient not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving patient details.");
            }
        }
        [HttpGet("GetPatientPrescription")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientPrescription(string patientId)
        {
            if (string.IsNullOrEmpty(patientId))
            {
                return BadRequest("Patient ID cannot be null or empty.");
            }
            try
            {
                var result = await _doctorService.GetPatientPrescription(patientId);
                if (result == null)
                {
                    return NotFound("Prescription not found for the patient.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the patient's prescription.");
            }
        }
        [HttpGet("GetLabTests")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetLabTests()
        {
            try
            {
                var result = await _doctorService.GetLabTests();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving lab tests.");
            }
        }
    }
}
