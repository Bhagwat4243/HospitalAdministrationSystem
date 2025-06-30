
using HospitalManagementSystem.Bl.AuthRepo.IService;
using HospitalManagementSystem.Bl.AuthRepo.Service;
using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Db.Model.AuthModel;
using HospitalManagementSystem.Dto.AuthDto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementSystem.AuthApi
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
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<HMSDbContext>()
            .AddDefaultTokenProviders();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ITokenGenerator, TokenGenerator>();
            builder.Services.Configure<JwtOptions>(
          builder.Configuration.GetSection("ApiSettings:JwtOptions"));

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
