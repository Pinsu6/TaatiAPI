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
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))  // Keep PascalCase for now
                .ForMember(dest => dest.CusCode, opt => opt.MapFrom(src => src.CusCode ?? string.Empty))
                .ForMember(dest => dest.CusFirstname, opt => opt.MapFrom(src => src.CusFirstname ?? string.Empty))
                .ForMember(dest => dest.CusLastname, opt => opt.MapFrom(src => src.CusLastname ?? string.Empty))
                .ForMember(dest => dest.CusMobileno, opt => opt.MapFrom(src => src.CusMobileno ?? string.Empty))
                .ForMember(dest => dest.CusPhonenoO, opt => opt.MapFrom(src => src.CusPhonenoO ?? string.Empty))
                .ForMember(dest => dest.CusPhonenoR, opt => opt.MapFrom(src => src.CusPhonenoR ?? string.Empty))
                .ForMember(dest => dest.CusEmail, opt => opt.MapFrom(src => src.CusEmail ?? string.Empty))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City ?? string.Empty))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address ?? string.Empty))
                .ForMember(dest => dest.Pin, opt => opt.MapFrom(src => src.Pin ?? string.Empty))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.District ?? string.Empty))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country ?? string.Empty))
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.BitIsActive, opt => opt.MapFrom(src => src.BitIsActive))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.BitIsDelete, opt => opt.MapFrom(src => src.BitIsDelete))
                .ForMember(dest => dest.StoreAmtremain, opt => opt.MapFrom(src => src.StoreAmtremain))
                .ForMember(dest => dest.StoreAmtused, opt => opt.MapFrom(src => src.StoreAmtused))
                .ForMember(dest => dest.Region, opt => opt.MapFrom(src => src.Region ?? string.Empty))
                .ForMember(dest => dest.Pbsllicense, opt => opt.MapFrom(src => src.Pbsllicense ?? string.Empty))
                .ForMember(dest => dest.LicenseType, opt => opt.MapFrom(src => src.LicenseType ?? string.Empty))
                .ForMember(dest => dest.LicenseExpiry, opt => opt.MapFrom(src => src.LicenseExpiry))
                .ForMember(dest => dest.CusTypeId, opt => opt.MapFrom(src => src.CusTypeId))
                .ForMember(dest => dest.Creditlim, opt => opt.MapFrom(src => src.Creditlim))
                .ForMember(dest => dest.Creditdays, opt => opt.MapFrom(src => src.Creditdays));

            CreateMap<Customer, CustomerDetailDto>()
                .IncludeBase<Customer, CustomerResponseDto>()
                .ForMember(dest => dest.CustomerType, opt => opt.MapFrom(src => src.CustomerType))
                .ForMember(dest => dest.Employee, opt => opt.MapFrom(src => src.Employee))
                .ForMember(dest => dest.TotalOrders, opt => opt.Ignore())
                .ForMember(dest => dest.LifetimeValue, opt => opt.Ignore())
                .ForMember(dest => dest.LastPurchase, opt => opt.Ignore())
                .ForMember(dest => dest.ActivePolicies, opt => opt.Ignore())
                .ForMember(dest => dest.OrderHistory, opt => opt.Ignore())
                .ForMember(dest => dest.PurchaseTrend, opt => opt.Ignore())
                .ForMember(dest => dest.CategorySplit, opt => opt.Ignore())
                .ForMember(dest => dest.Engagement, opt => opt.Ignore());

            CreateMap<CustomerType, CustomerTypeDto>()
                .ForMember(dest => dest.CusTypeName, opt => opt.MapFrom(src => src.CusTypeName ?? string.Empty))
                .ForMember(dest => dest.BitIsActive, opt => opt.MapFrom(src => src.BitIsActive))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.BitIsDelete, opt => opt.MapFrom(src => src.BitIsDelete));

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.EmployeeName))
                .ForMember(dest => dest.EmpShort, opt => opt.MapFrom(src => src.EmpShort))
                .ForMember(dest => dest.EmpPerson, opt => opt.MapFrom(src => src.EmpPerson ?? string.Empty))
                .ForMember(dest => dest.EmpPosition, opt => opt.MapFrom(src => src.EmpPosition ?? string.Empty))
                .ForMember(dest => dest.EmpEmail, opt => opt.MapFrom(src => src.EmpEmail ?? string.Empty))
                .ForMember(dest => dest.EmpMobile, opt => opt.MapFrom(src => src.EmpMobile ?? string.Empty))
                .ForMember(dest => dest.EmpAddress, opt => opt.MapFrom(src => src.EmpAddress ?? string.Empty))
                .ForMember(dest => dest.EmpType, opt => opt.MapFrom(src => src.EmpType ?? string.Empty))
                .ForMember(dest => dest.BitIsActive, opt => opt.MapFrom(src => src.BitIsActive))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                .ForMember(dest => dest.BitIsDelete, opt => opt.MapFrom(src => src.BitIsDelete))
                .ForMember(dest => dest.EmpPassword, opt => opt.MapFrom(src => src.EmpPassword ?? string.Empty))
                .ForMember(dest => dest.BintUserId, opt => opt.MapFrom(src => src.BintUserId))
                .ForMember(dest => dest.EmpStartDate, opt => opt.MapFrom(src => src.EmpStartDate));
        }
    }
}
