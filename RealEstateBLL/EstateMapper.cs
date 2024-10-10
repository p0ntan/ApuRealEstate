// Created by Pontus Åkerberb 2024-10-07
using AutoMapper;
using RealEstateDTO;
using RealEstateBLL.Estates;
using RealEstateBLL.Persons;
using RealEstateBLL.Payments;

namespace RealEstateBLL;

public class EstateMapper
{
    private IMapper _mapper;

    public EstateMapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Address, AddressDTO>();
            cfg.CreateMap<Person, PersonDTO>().IncludeAllDerived();
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
            cfg.CreateMap<Hotel, HotelDTO>();
            cfg.CreateMap<Factory, FactoryDTO>();
            cfg.CreateMap<Warehouse, WarehouseDTO>();
            cfg.CreateMap<AddressDTO, Address>();
            cfg.CreateMap<PersonDTO, Person>().IncludeAllDerived();
            cfg.CreateMap<SellerDTO, Seller>();
            cfg.CreateMap<BuyerDTO, Buyer>()
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment));
            cfg.CreateMap<PaymentDTO, Payment>()
                .IncludeAllDerived()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount));
            cfg.CreateMap<BankDTO, Bank>();
            cfg.CreateMap<PaypalDTO, Paypal>();
            cfg.CreateMap<WesternUnionDTO, WesternUnion>();
            cfg.CreateMap<EstateDTO, Estate>()
                .IncludeAllDerived()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Seller, opt => opt.MapFrom(src => src.Seller))
                .ForMember(dest => dest.Buyer, opt => opt.MapFrom(src => src.Buyer));
            cfg.CreateMap<ResidentialDTO, Residential>()
                .IncludeAllDerived();
            cfg.CreateMap<CommercialDTO, Commercial>()
                .IncludeAllDerived();
            cfg.CreateMap<InstitutionalDTO, Institutional>()
                .IncludeAllDerived();
            cfg.CreateMap<VillaDTO, Villa>()
                .IncludeAllDerived();
            cfg.CreateMap<RowhouseDTO, Rowhouse>();
            cfg.CreateMap<ApartmentDTO, Apartment>()
                .IncludeAllDerived();
            cfg.CreateMap<RentalDTO, Rental>();
            cfg.CreateMap<TenementDTO, Tenement>();
            cfg.CreateMap<HospitalDTO, Hospital>();
            cfg.CreateMap<SchoolDTO, School>();
            cfg.CreateMap<UniversityDTO, University>();
            cfg.CreateMap<ShopDTO, Shop>();
            cfg.CreateMap<HotelDTO, Hotel>();
            cfg.CreateMap<FactoryDTO, Factory>();
            cfg.CreateMap<WarehouseDTO, Warehouse>();
        });

        config.AssertConfigurationIsValid();

        _mapper = config.CreateMapper();
    }

    public TDestination MapDTO<TSource, TDestination>(TSource source)
    {
        return _mapper.Map<TDestination>(source);
    }
}
