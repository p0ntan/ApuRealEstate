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
            cfg.CreateMap<Person, PersonDTO>();
            cfg.CreateMap<Seller, SellerDTO>();
            cfg.CreateMap<Buyer, BuyerDTO>()
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment)); ;
            cfg.CreateMap<Payment, PaymentDTO>();
            cfg.CreateMap<Bank, BankDTO>();
            cfg.CreateMap<Paypal, PaypalDTO>();
            cfg.CreateMap<WesternUnion, WesternUnionDTO>();
            cfg.CreateMap<Estate, EstateDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller))
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer));
            cfg.CreateMap<Residential, ResidentialDTO>();
            cfg.CreateMap<Commercial, CommercialDTO>();
            cfg.CreateMap<Institutional, InstitutionalDTO>();
            cfg.CreateMap<Villa, VillaDTO>();
            cfg.CreateMap<Rowhouse, RowhouseDTO>();
            cfg.CreateMap<Apartment, ApartmentDTO>();
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
