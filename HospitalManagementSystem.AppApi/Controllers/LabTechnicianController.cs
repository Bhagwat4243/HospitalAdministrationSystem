using HospitalManagementSystem.Bl.AppRepo.LabTechnician.IService;
using HospitalManagementSystem.Dto.AppDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagementSystem.AppApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LabTechnicianController : ControllerBase
    {
        private readonly ILabTechnicianService _labTechnicianService;
        public LabTechnicianController(ILabTechnicianService labTechnicianService)
        {
            _labTechnicianService = labTechnicianService;
        }
        [HttpGet("GetReadyLabTests")]
        [Authorize(Roles = "LabTechnician")]
        public async Task<IActionResult> GetReadyLabTests()
        {
            try
            {
                var result = await _labTechnicianService.GetReadyLabTestsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while fetching ready lab tests.");
            }
        }
        [HttpPost("StartLabTest")]
        [Authorize(Roles = "LabTechnician")]
        public async Task<IActionResult> StartLabTest([FromBody] Guid labTestId)
        {
            if (labTestId == Guid.Empty)
            {
                return BadRequest("Invalid lab test ID.");
            }
            try
            {
                var result = await _labTechnicianService.StartLabTestAsync(labTestId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while starting the lab test.");
            }
        }
        [HttpPost("SubmitLabTestResult")]
        [Authorize(Roles = "LabTechnician")]
        public async Task<IActionResult> SubmitLabTestResult([FromBody] LabTestResultUploadDto dto)
        {
            if (dto == null || dto.LabTestId == Guid.Empty)
            {
                return BadRequest("Invalid lab test result data.");
            }
            try
            {
                var result = await _labTechnicianService.SubmitLabTestResultAsync(dto.LabTestId, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while submitting the lab test result.");
            }
        }
    }
}
