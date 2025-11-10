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

           
        }
    }
}
