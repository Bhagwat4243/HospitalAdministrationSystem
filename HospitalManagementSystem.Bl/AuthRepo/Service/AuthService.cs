using HospitalManagementSystem.Bl.AuthRepo.IService;
using HospitalManagementSystem.Db.Data;
using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Db.Model.AuthModel;
using HospitalManagementSystem.Dto.AuthDto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace HospitalManagementSystem.Bl.AuthRepo.Service
{
    public class AuthService : IAuthService
    {
        private readonly HMSDbContext _authDb;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ITokenGenerator tokenGenerator;

        public AuthService(HMSDbContext authDb,
            UserManager<ApplicationUser> userManager,
          RoleManager<IdentityRole> roleManager, ITokenGenerator tokenGenerator)
        {
            _authDb = authDb;
            _userManager = userManager;
            _roleManager = roleManager;
            this.tokenGenerator = tokenGenerator;
        }
        public async Task<bool> AssignRole(string email, string roleName)
        {
            try
            {
                var user=_authDb.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LoginResponseDto> Login(LoginDto loginDto)
        {
            try
            {

                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                {
                    throw new Exception("Invalid email or password");
                }
                var UserDto = new UserDto
                {
                    Name = user.Name,
                    Gender = user.Gender,
                    Age = user.Age,
                    AddharNo = user.AddharNo,
                    DOB = user.DOB,
                    State = user.State,
                    City = user.City,
                    PostalCode = user.PostalCode
                };
                var roles = await _userManager.GetRolesAsync(user);
                var loginResponse = new LoginResponseDto
                {
                    user = UserDto,
                    Token = tokenGenerator.GenerateToken(user, roles)
                };
                return loginResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<bool> Registration(ApplicationUser applicationUser, string password, List<string> roles)
        {
            try
            {
                var data = _authDb.ApplicationUsers.FirstOrDefault(u => u.Email != null && u.Email.ToLower() == applicationUser.Email.ToLower());
                if (data != null)
                {
                    throw new Exception("User already exists with this email");
                }

                var result = await _userManager.CreateAsync(applicationUser, password);
                if (!result.Succeeded)
                {
                    throw new Exception("registration failed");
                }
                foreach (var role in roles)
                {
                    await AssignRole(applicationUser.Email, role);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> AdminRegistration(AdminRegistrationDto adminRegistrationDto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = adminRegistrationDto.Email,
                    UserName = adminRegistrationDto.Email,
                    Age = adminRegistrationDto.Age,
                    Name = adminRegistrationDto.Name,
                    PostalCode = adminRegistrationDto.PostalCode,
                    State = adminRegistrationDto.State,
                    Gender=adminRegistrationDto.Gender,
                    City = adminRegistrationDto.City,
                    AddharNo = adminRegistrationDto.AddharNo,
                    DOB = adminRegistrationDto.DateOfBirth,
                    PhoneNumber = adminRegistrationDto.PhoneNo,                   
                };
                var result = await Registration(user, adminRegistrationDto.Password, adminRegistrationDto.Role);
                if (result)
                {
                    var newAdmin = new Admin
                    {
                        OwnerName = adminRegistrationDto.OwnerName,
                        OwnerContactNo = adminRegistrationDto.OwnerContactNo,
                        HospitalLicense = adminRegistrationDto.HospitalLicense,
                        GSTNumber = adminRegistrationDto.GSTNumber,
                        UserId = user.Id
                    };
                    await _authDb.Admin_Tbl.AddAsync(newAdmin);
                    await _authDb.SaveChangesAsync();
                    return "Admin Registration successful";
                }
                else
                {
                    throw new Exception("Registration failed");
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> DoctorRegistration(DoctorRegistrationDto doctorRegistrationDto)
        {
            try
            {
                var data = new ApplicationUser
                {
                    Email = doctorRegistrationDto.Email,
                    UserName = doctorRegistrationDto.Email,
                    Age = doctorRegistrationDto.Age,
                    Name = doctorRegistrationDto.Name,
                    PostalCode = doctorRegistrationDto.PostalCode,
                    AddharNo = doctorRegistrationDto.AddharNo,
                    State = doctorRegistrationDto.State,
                    City = doctorRegistrationDto.City,
                    Gender = doctorRegistrationDto.Gender,
                    DOB = doctorRegistrationDto.DateOfBirth,
                    PhoneNumber = doctorRegistrationDto.PhoneNo
                };
                var result = await Registration(data, doctorRegistrationDto.Password, doctorRegistrationDto.Role);
                if (result)
                {
                    var newDoctor = new Doctor
                    {
                        Specialization = doctorRegistrationDto.Specialization,
                        ExperienceYears = doctorRegistrationDto.ExperienceYears,
                        Qualifications =string.Join(",",doctorRegistrationDto.Qualifications),
                        AlternatePhoneNo = doctorRegistrationDto.AlternatePhoneNo,
                        LicenseNumber = doctorRegistrationDto.LicenseNumber,
                        LicenseExpiryDate = doctorRegistrationDto.LicenseExpiryDate,
                        AvailableDays=string.Join(",", doctorRegistrationDto.AvailableDays),
                        StartTime = doctorRegistrationDto.StartTime,
                        EndTime = doctorRegistrationDto.EndTime,
                        RoomNumber = doctorRegistrationDto.RoomNumber,
                        UserId = data.Id
                    };
                    await _authDb.Doctor_Tbl.AddAsync(newDoctor);
                    await _authDb.SaveChangesAsync();
                    return "Doctor Registration successful";
                }
                else
                {
                    throw new Exception("Registration failed");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> ChemistRegistration(ChemistRegistrationDto chemistRegistrationDto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = chemistRegistrationDto.Email,
                    UserName = chemistRegistrationDto.Email,
                    Age = chemistRegistrationDto.Age,
                    Name = chemistRegistrationDto.Name,
                    PostalCode = chemistRegistrationDto.PostalCode,
                    AddharNo = chemistRegistrationDto.AddharNo,
                    State = chemistRegistrationDto.State,
                    City = chemistRegistrationDto.City,
                    DOB = chemistRegistrationDto.DateOfBirth,
                    Gender = chemistRegistrationDto.Gender,
                    PhoneNumber = chemistRegistrationDto.PhoneNo

                };
                var result = await Registration(user, chemistRegistrationDto.Password, chemistRegistrationDto.Role);
                if (result)
                {
                    var newChemist = new Chemist
                    {
                        Qualification = string.Join(",",chemistRegistrationDto.Qualification),
                        LicenseNumber = chemistRegistrationDto.LicenseNumber,
                        LicenseExpiryDate = chemistRegistrationDto.LicenseExpiryDate,
                        LicenseIssuedBy = chemistRegistrationDto.LicenseIssuedBy,
                        ExperienceInYears = chemistRegistrationDto.ExperienceInYears,
                        UserId = user.Id
                    };
                    await _authDb.Chemist_Tbl.AddAsync(newChemist);
                    await _authDb.SaveChangesAsync();
                    return "Chemist Registration successful";
                }
                else
                {
                    throw new Exception("Registration failed");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> LabTechnicianRegistration(LabTechnicianRegistrationDto labTechnicianRegistrationDto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = labTechnicianRegistrationDto.Email,
                    UserName = labTechnicianRegistrationDto.Email,
                    Age = labTechnicianRegistrationDto.Age,
                    Name = labTechnicianRegistrationDto.Name,
                    PostalCode = labTechnicianRegistrationDto.PostalCode,
                    AddharNo = labTechnicianRegistrationDto.AddharNo,
                    State = labTechnicianRegistrationDto.State,
                    City = labTechnicianRegistrationDto.City,
                    Gender = labTechnicianRegistrationDto.Gender,
                    DOB = labTechnicianRegistrationDto.DateOfBirth,
                    PhoneNumber = labTechnicianRegistrationDto.PhoneNo,
                };
                var result = await Registration(user, labTechnicianRegistrationDto.Password, labTechnicianRegistrationDto.Role);
                if (result)
                {
                    var newLabTechnician = new LabTechnician
                    {
                        Qualification =string.Join(",",labTechnicianRegistrationDto.Qualification),
                        Specialization = labTechnicianRegistrationDto.Specialization,
                        ExperienceInYears = labTechnicianRegistrationDto.ExperienceInYears,
                        LicenseNumber = labTechnicianRegistrationDto.LicenseNumber,
                        LicenseExpiryDate = labTechnicianRegistrationDto.LicenseExpiryDate,
                        LicenseIssuedBy=labTechnicianRegistrationDto.LicenseIssuedBy,
                        JoiningDate = labTechnicianRegistrationDto.JoiningDate,
                        UserId = user.Id
                    };
                    await _authDb.LabTechnician_Tbl.AddAsync(newLabTechnician);
                    await _authDb.SaveChangesAsync();
                    return "Lab Technician Registration successful";
                }
                else
                {
                    throw new Exception("Registration failed");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> NurseRegistration(NurseRegistrationDto nurseRegistrationDto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = nurseRegistrationDto.Email,
                    UserName = nurseRegistrationDto.Email,
                    Name = nurseRegistrationDto.Name,
                    Age = nurseRegistrationDto.Age,
                    PostalCode = nurseRegistrationDto.PostalCode,
                    AddharNo = nurseRegistrationDto.AddharNo,
                    State = nurseRegistrationDto.State,
                    City = nurseRegistrationDto.City,
                    Gender = nurseRegistrationDto.Gender,
                    DOB = nurseRegistrationDto.DateOfBirth,
                    PhoneNumber = nurseRegistrationDto.PhoneNo
                };
                var result = await Registration(user, nurseRegistrationDto.Password, nurseRegistrationDto.Role);
                if (result)
                {
                    var newNurse = new Nurse
                    {
                        Qualification = string.Join(",", nurseRegistrationDto.Qualification),
                        ExperienceInYears = nurseRegistrationDto.ExperienceInYears,
                        RegistrationCouncil = nurseRegistrationDto.RegistrationCouncil,
                        Departments = nurseRegistrationDto.Departments,
                        CouncilRegistrationNumber = nurseRegistrationDto.CouncilRegistrationNumber,
                        UserId = user.Id
                    };
                    await _authDb.Nurse_Tbl.AddAsync(newNurse);
                    await _authDb.SaveChangesAsync();
                    return "Nurse Registration successful";
                }
                else
                {
                    throw new Exception("Registration failed");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> RegisterPatient(PatientRegistrationDto patientRegistrationDto)
        {
            try
            {
                var user = new ApplicationUser
                {
                    Email = patientRegistrationDto.Email,
                    UserName = patientRegistrationDto.Email,
                    Name = patientRegistrationDto.Name,
                    Age = patientRegistrationDto.Age,
                    PostalCode = patientRegistrationDto.PostalCode,
                    AddharNo = patientRegistrationDto.AddharNo,
                    State = patientRegistrationDto.State,
                    City = patientRegistrationDto.City,
                    Gender = patientRegistrationDto.Gender,
                    DOB = patientRegistrationDto.DateOfBirth,
                    PhoneNumber = patientRegistrationDto.PhoneNo
                };
                var result = await Registration(user, patientRegistrationDto.Password, patientRegistrationDto.Role);
                if (result)
                {
                    var newPatient = new Patient
                    {
                        BloodGroup = patientRegistrationDto.BloodGroup,
                        MedicalHistory = patientRegistrationDto.MedicalHistory,
                        ChronicDiseases = patientRegistrationDto.ChronicDiseases,
                        RegistrationDate = patientRegistrationDto.RegistrationDate,
                        UserId = user.Id
                    };
                    await _authDb.Patient_Tbl.AddAsync(newPatient);
                    await _authDb.SaveChangesAsync();
                    return "Patient Registration successful";
                }
                else
                {
                    throw new Exception("Registration failed");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
