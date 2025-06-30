using HospitalManagementSystem.Bl.AppRepo.Admin.IService;
using HospitalManagementSystem.Dto.AppDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.AppApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors()
        {
            try
            {
                var doctors = await _adminService.GetAllDoctor();
                if (doctors == null || !doctors.Any())
                {
                    return NotFound("No doctors found.");
                }
                return Ok(doctors);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching doctors.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            try
            {
                var patients = await _adminService.GetAllPatient();
                if (patients == null || !patients.Any())
                {
                    return NotFound("No patients found.");
                }
                return Ok(patients);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching patients.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllChemists()
        {
            try
            {
                var chemists = await _adminService.GetAllChemist();
                if (chemists == null || !chemists.Any())
                {
                    return NotFound("No chemists found.");
                }
                return Ok(chemists);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching chemists.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllLabTechnicians()
        {
            try
            {
                var labTechnicians = await _adminService.GetAllLabTechnician();
                if (labTechnicians == null || !labTechnicians.Any())
                {
                    return NotFound("No lab technicians found.");
                }
                return Ok(labTechnicians);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching lab technicians.");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllNurses()
        {
            try
            {
                var nurses = await _adminService.GetAllNurse();
                if (nurses == null || !nurses.Any())
                {
                    return NotFound("No nurses found.");
                }
                return Ok(nurses);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching nurses.");
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLabTest([FromBody] LabTestDto labTestDto)
        {
            if (labTestDto == null)
            {
                return BadRequest("Lab test details cannot be null.");
            }
            try
            {
                var result = await _adminService.CreateLabTest(labTestDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the lab test.");
            }
        }
    }
}
