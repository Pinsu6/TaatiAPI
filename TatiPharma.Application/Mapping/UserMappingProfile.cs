using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatiPharma.Application.DTOs;
using TatiPharma.Domain.Entities;


namespace TatiPharma.Application.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName ?? string.Empty))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress ?? string.Empty))
                .ForMember(dest => dest.StrFirstName, opt => opt.MapFrom(src => src.StrFirstName ?? string.Empty))
                .ForMember(dest => dest.StrLastName, opt => opt.MapFrom(src => src.StrLastName ?? string.Empty))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber ?? string.Empty))
                .ForMember(dest => dest.StrMobileNumber, opt => opt.MapFrom(src => src.StrMobileNumber ?? string.Empty))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender ?? string.Empty))
                .ForMember(dest => dest.IsMain, opt => opt.MapFrom(src => src.IsMain));

            //CreateMap<Customer, CustomerResponseDto>()
            //    .ForMember(dest => dest.CusCode, opt => opt.MapFrom(src => src.CusCode ?? string.Empty))
            //    .ForMember(dest => dest.CusFirstname, opt => opt.MapFrom(src => src.CusFirstname ?? string.Empty))
            //    .ForMember(dest => dest.CusLastname, opt => opt.MapFrom(src => src.CusLastname ?? string.Empty))
            //    .ForMember(dest => dest.CusMobileno, opt => opt.MapFrom(src => src.CusMobileno ?? string.Empty))
            //    .ForMember(dest => dest.CusPhonenoO, opt => opt.MapFrom(src => src.CusPhonenoO ?? string.Empty))
            //    .ForMember(dest => dest.CusPhonenoR, opt => opt.MapFrom(src => src.CusPhonenoR ?? string.Empty))
            //    .ForMember(dest => dest.CusEmail, opt => opt.MapFrom(src => src.CusEmail ?? string.Empty))
            //    .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City ?? string.Empty))
            //    .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? string.Empty))
            //    .ForMember(dest => dest.Pin, opt => opt.MapFrom(src => src.Pin ?? string.Empty))
            //    .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District ?? string.Empty))
            //    .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country ?? string.Empty))
            //    .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region ?? string.Empty))
            //    .ForMember(dest => dest.Pbsllicense, opt => opt.MapFrom(src => src.Pbsllicense ?? string.Empty))
            //    .ForMember(dest => dest.LicenseType, opt => opt.MapFrom(src => src.LicenseType ?? string.Empty));
        }
    }
}
