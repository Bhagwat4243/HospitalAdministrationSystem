using HospitalManagementSystem.Dto.AppDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.PdfGeneratorFolder.IService
{
    public interface IPdfGenerate
    {
        byte[]  GeneratePrescriptionPdf(PrescriptionPdfDto prescriptionPdfDto);
    }
}
