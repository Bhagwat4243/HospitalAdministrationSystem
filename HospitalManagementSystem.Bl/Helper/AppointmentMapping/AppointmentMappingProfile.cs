using AutoMapper;
using HospitalManagementSystem.Db.Model.AppModel;
using HospitalManagementSystem.Dto.AppDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HospitalManagementSystem.Bl.Helper.AppointmentMapping
{
    public class AppointmentMappingProfile:Profile
    {
        public AppointmentMappingProfile()
        {
            CreateMap<PrescriptionDetails, PrescriptionDetailsDto>()
                .ForMember(dest => dest.Medicine, opt => opt.MapFrom(src => src.MedicineName))
                .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
                .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
                .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
                .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
                .ReverseMap();

            CreateMap<Prescriptions, PrescriptionDto>()
                .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
                .ForMember(dest => dest.Note, opt => opt.MapFrom(src => src.Notes))
                .ReverseMap();

            CreateMap<PrescriptionDetails, MedicineInfoResponseDto>()
            .ForMember(dest => dest.MedicineName, opt => opt.MapFrom(src => src.MedicineName))
            .ForMember(dest => dest.Dosage, opt => opt.MapFrom(src => src.Dosage))
            .ForMember(dest => dest.Frequency, opt => opt.MapFrom(src => src.Frequency))
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration))
            .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions))
            .ReverseMap();

            CreateMap<Prescriptions, PrescriptionResponseDto>()
              .ForMember(dest => dest.PrescriptionId, opt => opt.MapFrom(src => src.PrescriptionId))
              .ForMember(dest => dest.AppointmentId, opt => opt.MapFrom(src => src.AppointmentId))
              .ForMember(dest => dest.PrescribeDate, opt => opt.MapFrom(src => src.PrescriptionDate))
             // .ForMember(dest => dest.MedicinesInfoDto, opt => opt.MapFrom(src => src.PrescriptionDetails))
            .ReverseMap();

            CreateMap<LabTest, LabTestDto>()
                .ReverseMap();

        }
    }
}
