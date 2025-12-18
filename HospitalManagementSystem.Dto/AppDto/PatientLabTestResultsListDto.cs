using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Dto.AppDto
{
    public class PatientLabTestResultsListDto
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public List<PatientLabTestResultDto> Results { get; set; } = new();
    }
}
