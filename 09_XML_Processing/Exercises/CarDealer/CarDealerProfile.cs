using AutoMapper;
using CarDealer.DTOs.Export.Cars;
using CarDealer.DTOs.Export.Parts;
using CarDealer.DTOs.Export.Suppliers;
using CarDealer.DTOs.Import.Cars;
using CarDealer.DTOs.Import.Customers;
using CarDealer.DTOs.Import.Parts;
using CarDealer.DTOs.Import.Sales;
using CarDealer.DTOs.Import.Suppliers;
using CarDealer.Models;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;

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
            this.CreateMap<ImportPartDTO, Part>()
                .ForMember(d => d.SupplierId, opt => opt.MapFrom(src => src.SupplierId!.Value));

            this.CreateMap<Part, ExportPartDTO>();

            // Cars
            /*this.CreateMap<ImportCarDTO, Car>()
                .ForMember(d => d.PartsCars, opt => opt.MapFrom(src => src.Parts.Select(p => new PartCar() { PartId = p.PartId })));*/

            this.CreateMap<ImportCarDTO, Car>()
                .ForSourceMember(src => src.Parts, opt => opt.DoNotValidate());

            this.CreateMap<Car, ExportCarWithDistanceDTO>();

            this.CreateMap<Car, ExportCarFromMakeBMWDTO>();

            this.CreateMap<Car, ExportCarWithItsListOfPartsDTO>()
                .ForMember(d => d.CarParts, opt => opt.MapFrom(src => src.PartsCars
                                                                         .Select(pc => pc.Part)
                                                                         .OrderByDescending(p => p.Price)
                                                                         .ToArray()));

            // Customers
            this.CreateMap<ImportCustomerDTO, Customer>()
                .ForMember(d => d.BirthDate, opt => opt.MapFrom(src => DateTime.Parse(src.BirthDate, CultureInfo.InvariantCulture)));

            // Sales
            this.CreateMap<ImportSaleDTO, Sale>()
                .ForMember(d => d.CarId, opt => opt.MapFrom(src => src.CarId!.Value));
        }
    }
}