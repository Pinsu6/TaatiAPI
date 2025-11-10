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
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<DrugMaster, ProductResponseDto>()
                .ForMember(dest => dest.DrugCode, opt => opt.MapFrom(src => src.DrugCode ?? string.Empty))
                .ForMember(dest => dest.DrugQuickcode, opt => opt.MapFrom(src => src.DrugQuickcode ?? string.Empty))
                .ForMember(dest => dest.DrugName, opt => opt.MapFrom(src => src.DrugName ?? string.Empty))
                .ForMember(dest => dest.DrugShort, opt => opt.MapFrom(src => src.DrugShort ?? string.Empty))
                .ForMember(dest => dest.Strength, opt => opt.MapFrom(src => src.Strength ?? string.Empty))
                .ForMember(dest => dest.BrandName, opt => opt.MapFrom(src => src.BrandName ?? string.Empty))
                .ForMember(dest => dest.QuantityPack, opt => opt.MapFrom(src => src.QuantityPack ?? string.Empty))
                .ForMember(dest => dest.MaxLevel, opt => opt.MapFrom(src => src.MaxLevel))
                .ForMember(dest => dest.MinLevel, opt => opt.MapFrom(src => src.MinLevel))
                .ForMember(dest => dest.Narcotics, opt => opt.MapFrom(src => src.Narcotics))
                .ForMember(dest => dest.UnitCost, opt => opt.MapFrom(src => src.UnitCost))
                .ForMember(dest => dest.Margin, opt => opt.MapFrom(src => src.Margin))
                .ForMember(dest => dest.BitIsActive, opt => opt.MapFrom(src => src.BitIsActive))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated))
                // Navigation Properties
                .ForMember(dest => dest.DrugTypeName,
                    opt => opt.MapFrom(src => src.DrugTypeMaster != null ? src.DrugTypeMaster.DrugTypeName ?? string.Empty : string.Empty))
                .ForMember(dest => dest.DosageFormName,
                    opt => opt.MapFrom(src => src.DosageForm != null ? src.DosageForm.DosageFormname ?? string.Empty : string.Empty))
                .ForMember(dest => dest.ManufacturerName,
                    opt => opt.MapFrom(src => src.Manufacturer != null ? src.Manufacturer.ManufacturerName ?? string.Empty : string.Empty));

            // List mapping (keep as-is, but add null checks)
            CreateMap<DrugMaster, ProductResponseDto>()
                .ForMember(dest => dest.DrugCode, opt => opt.MapFrom(src => src.DrugCode ?? string.Empty))
                // ... (all other list fields as before)
                .ForMember(dest => dest.DrugTypeName, opt => opt.MapFrom(src => src.DrugTypeMaster != null ? src.DrugTypeMaster.DrugTypeName ?? string.Empty : string.Empty))
                .ForMember(dest => dest.DosageFormName, opt => opt.MapFrom(src => src.DosageForm != null ? src.DosageForm.DosageFormname ?? string.Empty : string.Empty))
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.Manufacturer != null ? src.Manufacturer.ManufacturerName ?? string.Empty : string.Empty));

            // Detail mapping (inherits list + adds extras)
            CreateMap<DrugMaster, ProductDetailDto>()
                .IncludeBase<DrugMaster, ProductResponseDto>()  // Reuse list mappings
                .ForMember(dest => dest.TheRapeuticclass, opt => opt.MapFrom(src => src.TheRapeuticclass ?? string.Empty))
                .ForMember(dest => dest.DrugNdccode, opt => opt.MapFrom(src => src.DrugNdccode ?? string.Empty))
                .ForMember(dest => dest.DrugType, opt => opt.MapFrom(src => src.DrugType ?? string.Empty))
                .ForMember(dest => dest.BitIsDelete, opt => opt.MapFrom(src => src.BitIsDelete))
                .ForMember(dest => dest.DrugTypeId, opt => opt.MapFrom(src => src.DrugTypeId))
                .ForMember(dest => dest.DosageId, opt => opt.MapFrom(src => src.DosageId))
                .ForMember(dest => dest.ManufacturerId, opt => opt.MapFrom(src => src.ManufacturerId))
                .ForMember(dest => dest.UdiId, opt => opt.MapFrom(src => src.UdiId))
                .ForMember(dest => dest.UomId, opt => opt.MapFrom(src => src.UomId))
                .ForMember(dest => dest.TaxDetailsId, opt => opt.MapFrom(src => src.TaxDetailsId))
                .ForMember(dest => dest.Taxid, opt => opt.MapFrom(src => src.Taxid))
                .ForMember(dest => dest.StockSummary, opt => opt.Ignore())
                .ForMember(dest => dest.ActiveBatches, opt => opt.Ignore())
                .ForMember(dest => dest.Pricing, opt => opt.Ignore())
                .ForMember(dest => dest.TotalRevenue, opt => opt.Ignore())
                .ForMember(dest => dest.TurnoverRate, opt => opt.Ignore())
                .ForMember(dest => dest.MonthlySalesTrend, opt => opt.Ignore())
                .ForMember(dest => dest.RegionalSales, opt => opt.Ignore())
                .ForMember(dest => dest.RecentOrders, opt => opt.Ignore())
                .ForMember(dest => dest.StockMovements, opt => opt.Ignore())
                .ForMember(dest => dest.Alerts, opt => opt.Ignore())
                // Full Navigation Objects
                .ForMember(dest => dest.DrugTypeMaster, opt => opt.MapFrom(src => src.DrugTypeMaster))
                .ForMember(dest => dest.DosageForm, opt => opt.MapFrom(src => src.DosageForm))
                .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(src => src.Manufacturer));

            // Navigation DTOs
            CreateMap<DrugTypeMaster, DrugTypeMasterDto>()
                .ForMember(dest => dest.DrugTypeName, opt => opt.MapFrom(src => src.DrugTypeName ?? string.Empty));

            CreateMap<DosageForm, DosageFormDto>()
                .ForMember(dest => dest.DosageFormname, opt => opt.MapFrom(src => src.DosageFormname ?? string.Empty))
                .ForMember(dest => dest.DosageType, opt => opt.MapFrom(src => src.DosageType ?? string.Empty));

            CreateMap<Manufacturer, ManufacturerDto>()
                .ForMember(dest => dest.ManufacturerName, opt => opt.MapFrom(src => src.ManufacturerName ?? string.Empty))
                .ForMember(dest => dest.Mfg, opt => opt.MapFrom(src => src.Mfg ?? string.Empty))
                .ForMember(dest => dest.MfgPerson, opt => opt.MapFrom(src => src.MfgPerson ?? string.Empty))
                .ForMember(dest => dest.MfgEmail, opt => opt.MapFrom(src => src.MfgEmail ?? string.Empty))
                .ForMember(dest => dest.MfgMobile, opt => opt.MapFrom(src => src.MfgMobile ?? string.Empty));

            CreateMap<DrugMaster, ProductDetailDto>()
                .IncludeBase<DrugMaster, ProductResponseDto>()
                .ForMember(dest => dest.TheRapeuticclass, opt => opt.MapFrom(src => src.TheRapeuticclass ?? string.Empty))
                .ForMember(dest => dest.DrugNdccode, opt => opt.MapFrom(src => src.DrugNdccode ?? string.Empty))
                .ForMember(dest => dest.DrugType, opt => opt.MapFrom(src => src.DrugType ?? string.Empty))
                .ForMember(dest => dest.BitIsDelete, opt => opt.MapFrom(src => src.BitIsDelete))
                .ForMember(dest => dest.DrugTypeId, opt => opt.MapFrom(src => src.DrugTypeId))
                .ForMember(dest => dest.DosageId, opt => opt.MapFrom(src => src.DosageId))
                .ForMember(dest => dest.ManufacturerId, opt => opt.MapFrom(src => src.ManufacturerId))
                .ForMember(dest => dest.DrugTypeMaster, opt => opt.MapFrom(src => src.DrugTypeMaster))
                .ForMember(dest => dest.DosageForm, opt => opt.MapFrom(src => src.DosageForm))
                .ForMember(dest => dest.Manufacturer, opt => opt.MapFrom(src => src.Manufacturer));
        }
    }
}
