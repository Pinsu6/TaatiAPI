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
    public class HelperMappingProfile : Profile
    {
        public HelperMappingProfile()
        {
            CreateMap<DrugTypeMaster, DrugTypeDropdownDto>()
                .ForMember(dest => dest.DrugTypeId, opt => opt.MapFrom(src => src.DrugTypeId))
                .ForMember(dest => dest.DrugTypeName, opt => opt.MapFrom(src => src.DrugTypeName ?? string.Empty));

            CreateMap<DrugMaster, ProductDropdownDto>()
                .ForMember(dest => dest.drugId, opt => opt.MapFrom(src => src.DrugId))
                .ForMember(dest => dest.drugName, opt => opt.MapFrom(src => src.DrugName ?? string.Empty));
        }
    }
}
