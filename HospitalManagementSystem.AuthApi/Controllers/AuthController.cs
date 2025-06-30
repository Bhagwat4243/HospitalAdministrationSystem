using HospitalManagementSystem.Bl.AuthRepo.IService;
using HospitalManagementSystem.Dto.AuthDto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.AuthApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var response = await _authService.Login(loginDto);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationDto registerAdminDto)
        {
            try
            {
                var response = await _authService.AdminRegistration(registerAdminDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegisterDoctor(DoctorRegistrationDto doctorRegistrationDto)
        {
            try
            {
                var response = await _authService.DoctorRegistration(doctorRegistrationDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegisterChemist(ChemistRegistrationDto chemistRegistrationDto)
        {
            try
            {
                var response = await _authService.ChemistRegistration(chemistRegistrationDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegisterLabTechnician(LabTechnicianRegistrationDto labTechnicianRegistrationDto)
        {
            try
            {
                var response = await _authService.LabTechnicianRegistration(labTechnicianRegistrationDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult> RegisterNurse(NurseRegistrationDto nurseRegistrationDto)
        {
            try
            {
                var response = await _authService.NurseRegistration(nurseRegistrationDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        [HttpPost]
        public async Task<IActionResult>RegisterPatient(PatientRegistrationDto patientRegistrationDto)
        {
            try
            {
                var response = await _authService.RegisterPatient(patientRegistrationDto);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
