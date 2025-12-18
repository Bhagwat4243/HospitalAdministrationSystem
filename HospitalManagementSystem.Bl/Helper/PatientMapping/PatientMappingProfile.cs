using AutoMapper;
using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Dto.AppDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.Helper.PatientMapping
{
    public class PatientMappingProfile:Profile
    {
        public PatientMappingProfile()
        {
            CreateMap<Patient, PatientDetailsDto>()
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AppUser.Name))
               .ForMember(dest => dest.AadharNo, opt => opt.MapFrom(src => src.AppUser.AddharNo))
               //.ForMember(dest => dest.Aage, opt => opt.MapFrom(src => src.AppUser.Age))
               .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.AppUser.Gender))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.AppUser.Email))
               .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.AppUser.DOB))
               .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.AppUser.State))
               .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.AppUser.City))
               .ForMember(dest => dest.Pincode, opt => opt.MapFrom(src => src.AppUser.PostalCode))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.AppUser.PhoneNumber))
               .ReverseMap();
        }
    }
}
