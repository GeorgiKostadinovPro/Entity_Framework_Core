using AutoMapper;
using CarDealer.DTOs.Export.Cars;
using CarDealer.DTOs.Export.Customers;
using CarDealer.DTOs.Export.Parts;
using CarDealer.DTOs.Export.Suppliers;
using CarDealer.DTOs.Import.Customers;
using CarDealer.DTOs.Import.Parts;
using CarDealer.DTOs.Import.Sales;
using CarDealer.DTOs.Import.Suppliers;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            // Suppliers
            this.CreateMap<ImportSupplierDTO, Supplier>();

            this.CreateMap<Supplier, ExportLocalSupplierDTO>()
                .ForMember(d => d.PartsCount, opt => opt.MapFrom(src => src.Parts.Count));

            // Parts
            this.CreateMap<ImportPartDTO, Part>();

            this.CreateMap<Part, ExportPartDTO>();

            // Cars
            /*this.CreateMap<Car, ExportCarFromMakeToyotaDTO>()
                .ForMember(d => d.TraveledDistance, opt => opt.MapFrom(src => src.TravelledDistance));

            this.CreateMap<Car, ExportCarDTO>()
                .ForMember(d => d.TraveledDistance, opt => opt.MapFrom(src => src.TravelledDistance));*/

            this.CreateMap<Car, ExportCarWithItsListOfPartsDTO>()
                .ForMember(d => d.CarParts, opt => opt.MapFrom(src => src.PartsCars
                                                                         .Select(pc => pc.Part)
                                                                         .ToArray()));

            // Customers
            this.CreateMap<ImportCustomerDTO, Customer>();

            this.CreateMap<Customer, ExportOrderedCustomerDTO>();

            // Sales
            this.CreateMap<ImportSaleDTO, Sale>();
        }
    }
}