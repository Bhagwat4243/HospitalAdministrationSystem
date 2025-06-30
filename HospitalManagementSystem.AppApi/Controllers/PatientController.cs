using HospitalManagementSystem.Bl.AppRepo.Patient.IService;
using HospitalManagementSystem.Dto.AppDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalManagementSystem.AppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }
        [HttpPost("BookAppointment")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto bookAppointmentDto)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(patientId))
            {
                return Unauthorized("Invalid token or user ID not found.");
            }
            if (bookAppointmentDto == null || string.IsNullOrEmpty(patientId))
            {
                return BadRequest("Invalid appointment details or patient ID.");
            }
            try
            {
                var result = await _patientService.BookNewAppointment(bookAppointmentDto, patientId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status400BadRequest, "An error occurred while booking the appointment.");
            }
        }
        [HttpPost("CreateAppointmentPayment")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateAppointmentPayment([FromBody] AppointmentPaymentDto appointmentPaymentDto)
        {
            if (appointmentPaymentDto == null)
            {
                return BadRequest("Invalid payment details.");
            }
            try
            {
                var result = await _patientService.CreateAppointPayment(appointmentPaymentDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status400BadRequest, "An error occurred while processing the payment.");
            }
        }
        [HttpGet("GetDoctorDetails")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetDoctorDetails([FromQuery] string? Name = null, [FromQuery] string? Specialization = null)
        {
            try
            {
                var doctorDetails = await _patientService.GetDoctorDetails(Name, Specialization);
                if (doctorDetails == null)
                {
                    return NotFound("No doctor found with the provided criteria.");
                }
                return Ok(doctorDetails);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching doctor details.");
            }
        }
        [HttpGet("ShowBookAppoint")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> ShowBookAppoint()
        {
            try
            {
                var upcomingAppointments = await _patientService.ShowBookAppoint();
                return Ok(upcomingAppointments);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching upcoming appointments.");
            }
        }
        [HttpGet("GetPrescriptionList")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetPrescriptionList()
        {
            try
            {
                var prescriptions = await _patientService.GetPrescriptionList();
                return Ok(prescriptions);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching prescription list.");
            }
        }
        [HttpGet("GetAppointmentByPatientId")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetAppointmentByPatientId(string patientId)            
        {

            if (string.IsNullOrWhiteSpace(patientId))
            {
                return BadRequest("Patient ID cannot be null or empty.");
            }

            try
            {
                var appointments = await _patientService.GetAppointmentByPatientId(patientId);

                if (appointments == null || !appointments.Any())
                {
                    return NotFound("No appointments found for the provided patient ID.");
                }
                return Ok(appointments);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching appointments.");
            }
        }
        [HttpGet("DownloadPrescriptionPdf")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> DownloadPrescriptionPdf(Guid prescriptionId)
        {
            var patientId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(patientId))
            {
                return Unauthorized("Invalid token or user ID not found.");
            }
            try
            {
                var pdfBytes = await _patientService.DownloadPrescriptionPdf(prescriptionId, patientId);
                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    return NotFound("Prescription not found or no PDF available.");
                }
                return File(pdfBytes, "application/pdf", $"Prescription_{prescriptionId}.pdf");
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while downloading the prescription PDF.");
            }
        }
    }
}
