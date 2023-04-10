using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export.Cars;
using CarDealer.DTOs.Export.Customers;
using CarDealer.DTOs.Export.Parts;
using CarDealer.DTOs.Export.Suppliers;
using CarDealer.DTOs.Import.Cars;
using CarDealer.DTOs.Import.Customers;
using CarDealer.DTOs.Import.Parts;
using CarDealer.DTOs.Import.Sales;
using CarDealer.DTOs.Import.Suppliers;
using CarDealer.Models;
using Castle.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            // 01. Setup Database
            using CarDealerContext dbContext = new CarDealerContext();

            /*dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();*/

            // Import Data
            // 09. Import Suppliers
            /*string suppliersJson = File.ReadAllText(@"../../../Datasets/suppliers.json");
            string importSuppliersResult = ImportSuppliers(dbContext, suppliersJson);
            Console.WriteLine(importSuppliersResult);*/

            // 10. Import Parts
            /*string partsJson = File.ReadAllText(@"../../../Datasets/parts.json");
            string importPartsResult = ImportParts(dbContext, partsJson);
            Console.WriteLine(importPartsResult);*/

            // 11. Import Cars
            /*string carsJson = File.ReadAllText(@"../../../Datasets/cars.json");
            string importCarsResult = ImportCars(dbContext, carsJson);
            Console.WriteLine(importCarsResult);*/

            // 12. Import Customers
            /*string customersJson = File.ReadAllText(@"../../../Datasets/customers.json");
            string importCustomersResult = ImportCustomers(dbContext, customersJson);
            Console.WriteLine(importCustomersResult);*/

            // 13. Import Sales
            /*string salesJson = File.ReadAllText(@"../../../Datasets/sales.json");
            string importSalesResult = ImportSales(dbContext, salesJson);
            Console.WriteLine(importSalesResult);*/

            // Export

            // 14. Export Ordered Customers
            /*string exportOrderedCustomersResult = GetOrderedCustomers(dbContext);
            Console.WriteLine(exportOrderedCustomersResult);*/

            // 15. Export Cars From Make Toyota
            /*string exportCarsFromMakeToyotaResult = GetCarsFromMakeToyota(dbContext);
            Console.WriteLine(exportCarsFromMakeToyotaResult);*/

            // 16. Export Local Suppliers
            /*string exportLocalSuppliersResult = GetLocalSuppliers(dbContext);
            Console.WriteLine(exportLocalSuppliersResult);*/

            // 17. Export Cars with Their List of Parts
            string exportCarsWithTheirListOfPartsResult = GetCarsWithTheirListOfParts(dbContext);
            Console.WriteLine(exportCarsWithTheirListOfPartsResult);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            IEnumerable<ImportSupplierDTO> deserializedSuppliers
                = JsonConvert.DeserializeObject<IEnumerable<ImportSupplierDTO>>(inputJson);

            HashSet<Supplier> suppliersToImport = mapper.Map<HashSet<Supplier>>(deserializedSuppliers);

            context.Suppliers.AddRange(suppliersToImport);
            context.SaveChanges();

            return $"Successfully imported {suppliersToImport.Count}.";
        }

        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            IEnumerable<ImportPartDTO> deserializedParts
                = JsonConvert.DeserializeObject<IEnumerable<ImportPartDTO>>(inputJson);

            HashSet<Part> partsToImport = new HashSet<Part>();

            foreach (var partToImport in deserializedParts)
            {
                if (!context.Suppliers.Any(s => s.Id == partToImport.SupplierId))
                {
                    continue;
                }

                Part part = mapper.Map<Part>(partToImport);
                partsToImport.Add(part);
            }

            context.Parts.AddRange(partsToImport);
            context.SaveChanges();

            return $"Successfully imported {partsToImport.Count}."; ;
        }

        /*public static string ImportCars(CarDealerContext context, string inputJson)
        {
            IEnumerable<ImportCarDTO> deserializedCars
                = JsonConvert.DeserializeObject<IEnumerable<ImportCarDTO>>(inputJson);

            foreach (var carToImport in deserializedCars)
            {
                Car car = new Car
                {
                    Make = carToImport.Make,
                    Model = carToImport.Model,
                    TravelledDistance = carToImport.TravelledDistance
                };

                context.Cars.Add(car);
                context.SaveChanges();

                foreach (var partToImportId in carToImport.PartsId.Distinct())
                {
                    car.PartsCars.Add(new PartCar
                    {
                        PartId = partToImportId,
                        CarId = car.Id
                    }); 
                    
                    context.SaveChanges();
                }
            }

            return $"Successfully imported {context.Cars.Count()}.";
        }*/

        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            IEnumerable<ImportCustomerDTO> deserializedCustomers
                = JsonConvert.DeserializeObject<IEnumerable<ImportCustomerDTO>>(inputJson);

            HashSet<Customer> customersToImport = mapper.Map<HashSet<Customer>>(deserializedCustomers);

            context.Customers.AddRange(customersToImport);
            context.SaveChanges();

            return $"Successfully imported {customersToImport.Count}.";
        }

        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            IEnumerable<ImportSaleDTO> deserializedSales
                = JsonConvert.DeserializeObject<IEnumerable<ImportSaleDTO>>(inputJson);

            HashSet<Sale> salesToImport = mapper.Map<HashSet<Sale>>(deserializedSales);

            context.Sales.AddRange(salesToImport);
            context.SaveChanges();

            return $"Successfully imported {salesToImport.Count}.";
        }

        public static string GetOrderedCustomers(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var orderedCustomersToExport = context.Customers
                .AsNoTracking()
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .ProjectTo<ExportOrderedCustomerDTO>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(orderedCustomersToExport, Formatting.Indented);
        }

        /*public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var carsFromMakeToyotaToExport = context.Cars
                .AsNoTracking()
                .Where(c => c.Make == "Toyota")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .Select(c => new ExportCarFromMakeToyotaDTO
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TravelledDistance,
                })
                .ToArray();

            return JsonConvert.SerializeObject(carsFromMakeToyotaToExport, Formatting.Indented);
        }*/

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var localSuppliersToExport = context.Suppliers
                .AsNoTracking()
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSupplierDTO>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(localSuppliersToExport, Formatting.Indented);
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var carsWithTheirListOfPartsToExport = context.Cars
                .AsNoTracking()
                .Select(c => new ExportCarWithItsListOfPartsDTO
                {
                    Car = new ExportCarDTO
                    {
                        Make = c.Make,
                        Model = c.Model,
                        TraveledDistance = c.TraveledDistance
                    },
                    CarParts = c.PartsCars
                                .Select(pc => new ExportPartDTO
                                {
                                    Name = pc.Part.Name,
                                    Price = pc.Part.Price.ToString("f2")
                                })
                                .ToArray()
                })
                .ToArray();

            return JsonConvert.SerializeObject(carsWithTheirListOfPartsToExport, Formatting.Indented);
        }

        private static IMapper CreateMapper()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            }));

            return mapper;
        }
    }
}