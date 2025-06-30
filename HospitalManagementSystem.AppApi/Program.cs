
using HospitalManagementSystem.AppApi.Extension;
using HospitalManagementSystem.Bl.AppRepo.Admin.Implementation;
using HospitalManagementSystem.Bl.AppRepo.Admin.IService;
using HospitalManagementSystem.Bl.AppRepo.Doctor.Implementation;
using HospitalManagementSystem.Bl.AppRepo.Doctor.IService;
using HospitalManagementSystem.Bl.AppRepo.LabTechnician.Implementation;
using HospitalManagementSystem.Bl.AppRepo.LabTechnician.IService;
using HospitalManagementSystem.Bl.AppRepo.Patient.Implementation;
using HospitalManagementSystem.Bl.AppRepo.Patient.IService;
using HospitalManagementSystem.Bl.AuthRepo.IService;
using HospitalManagementSystem.Bl.AuthRepo.Service;
using HospitalManagementSystem.Bl.PdfGeneratorFolder.Implementation;
using HospitalManagementSystem.Bl.PdfGeneratorFolder.IService;
using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Db.Model.AuthModel;
using HospitalManagementSystem.Dto.AuthDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.AppApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<HMSDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                    opt => opt.MigrationsAssembly("HospitalManagementSystem.AppApi"));
            });
            builder.Services.AddAuthentication();
            builder.AddTokenConfiguration();
            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IDoctorService,DoctorService>();
            builder.Services.AddScoped<ILabTechnicianService, LabTechnicianService>();
            builder.Services.AddScoped<IPdfGenerate, PdfGenerate>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
