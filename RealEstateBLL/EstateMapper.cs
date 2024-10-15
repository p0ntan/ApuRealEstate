// Created by Pontus Åkerberb 2024-10-07
using AutoMapper;
using RealEstateDTO;
using RealEstateBLL.Estates;
using RealEstateBLL.Persons;
using RealEstateBLL.Payments;

namespace RealEstateBLL;

/// <summary>
/// Estatemapper maps between DTOs and Estates.
/// </summary>
public class EstateMapper
{
    private IMapper _mapper;

    /// <summary>
    /// Default constructor that sets upp the config for the AutoMapper.
    /// </summary>
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

        _mapper = config.CreateMapper();
    }

    /// <summary>
    /// Generic method to map between DTOs and Estates with it's nested types.
    /// </summary>
    /// <typeparam name="TSource">The type source</typeparam>
    /// <typeparam name="TDestination">The type destination</typeparam>
    /// <param name="source">Source to map from</param>
    /// <returns>The destinaion type</returns>
    public TDestination MapDTO<TSource, TDestination>(TSource source)
    {
        return _mapper.Map<TDestination>(source);
    }

    /// <summary>
    /// Maps a given DTO to it corresponding concrete estate type.
    /// </summary>
    /// <param name="estateDTO">The DTO to map from</param>
    /// <returns>The created estate, null if no match or any error.</returns>
    public Estate? MapDTOToEstate(EstateDTO estateDTO)
    {
        try
        {
            Estate? estate = estateDTO switch
            {
                RowhouseDTO rowhouse => MapDTO<RowhouseDTO, Rowhouse>(rowhouse),
                VillaDTO villa => MapDTO<VillaDTO, Villa>(villa),
                RentalDTO rental => MapDTO<RentalDTO, Rental>(rental),
                TenementDTO tenement => MapDTO<TenementDTO, Tenement>(tenement),
                HospitalDTO hospital => MapDTO<HospitalDTO, Hospital>(hospital),
                SchoolDTO school => MapDTO<SchoolDTO, School>(school),
                UniversityDTO university => MapDTO<UniversityDTO, University>(university),
                ShopDTO shop => MapDTO<ShopDTO, Shop>(shop),
                FactoryDTO factory => MapDTO<FactoryDTO, Factory>(factory),
                HotelDTO hotel => MapDTO<HotelDTO, Hotel>(hotel),
                WarehouseDTO warehouse => MapDTO<WarehouseDTO, Warehouse>(warehouse),
                _ => null
            };

            return estate;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Maps a given estate to it corresponding DTO.
    /// </summary>
    /// <param name="estate">The estate to map from</param>
    /// <returns>The created DTO, null if no match or any error.</returns>
    public EstateDTO? MapEstateToDTO(Estate estate)
    {
        try
        {
            EstateDTO? estateDTO = estate switch
            {
                Rowhouse rowhouse => MapDTO<Rowhouse, RowhouseDTO>(rowhouse),
                Villa villa => MapDTO<Villa, VillaDTO>(villa),
                Rental rental => MapDTO<Rental, RentalDTO>(rental),
                Tenement tenement => MapDTO<Tenement, TenementDTO>(tenement),
                Hospital hospital => MapDTO<Hospital, HospitalDTO>(hospital),
                School school => MapDTO<School, SchoolDTO>(school),
                University university => MapDTO<University, UniversityDTO>(university),
                Shop shop => MapDTO<Shop, ShopDTO>(shop),
                Factory factory => MapDTO<Factory, FactoryDTO>(factory),
                Hotel hotel => MapDTO<Hotel, HotelDTO>(hotel),
                Warehouse warehouse => MapDTO<Warehouse, WarehouseDTO>(warehouse),
                _ => null
            };

            return estateDTO;
        }
        catch
        {
            return null;
        }
    }
}
