using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class LabTestResultUploadDto
    {
        public Guid LabTestId { get; set; }
        public string Result { get; set; } = string.Empty;
        public IFormFile? ReportFile { get; set; }
    }
}
