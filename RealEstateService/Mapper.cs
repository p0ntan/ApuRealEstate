// Created by Pontus Åkerberb 2024-10-07
using AutoMapper;
using RealEstateDTO;
using RealEstateBLL;
using RealEstateBLL.Estates;
using RealEstateBLL.Persons;
using RealEstateBLL.Payments;

namespace RealEstateService;

public class Mapper
{
    private IMapper _mapper;

    public Mapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Address, AddressDTO>();
            cfg.CreateMap<Person, PersonDTO>()
                .IncludeAllDerived();
            cfg.CreateMap<Seller, SellerDTO>();
            cfg.CreateMap<Buyer, BuyerDTO>()
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment));
            cfg.CreateMap<Payment, PaymentDTO>()
                .IncludeAllDerived()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
            cfg.CreateMap<Bank, BankDTO>();
            cfg.CreateMap<Paypal, PaypalDTO>();
            cfg.CreateMap<WesternUnion, WesternUnionDTO>();
            cfg.CreateMap<Estate, EstateDTO>()
                .IncludeAllDerived()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller))
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer));
            cfg.CreateMap<Residential, ResidentialDTO>()
                .IncludeAllDerived();
            cfg.CreateMap<Commercial, CommercialDTO>()
                .IncludeAllDerived();
            cfg.CreateMap<Institutional, InstitutionalDTO>()
                .IncludeAllDerived();
            cfg.CreateMap<Villa, VillaDTO>()
                .IncludeAllDerived();
            cfg.CreateMap<Rowhouse, RowhouseDTO>();
            cfg.CreateMap<Apartment, ApartmentDTO>()
                .IncludeAllDerived();
            cfg.CreateMap<Rental, RentalDTO>();
            cfg.CreateMap<Tenement, TenementDTO>();
            cfg.CreateMap<Hospital, HospitalDTO>();
            cfg.CreateMap<School, SchoolDTO>();
            cfg.CreateMap<University, UniversityDTO>();
            cfg.CreateMap<Shop, ShopDTO>();
            cfg.CreateMap<Factory, FactoryDTO>();
            cfg.CreateMap<Warehouse, WarehouseDTO>();
        });

        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();
    }

    public TDestination MapToDTO<TSource, TDestination>(TSource source)
    {
        return _mapper.Map<TDestination>(source);
    }
}
