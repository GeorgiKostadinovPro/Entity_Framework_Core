using AutoMapper;
using AutoMapper.QueryableExtensions;
using CarDealer.Data;
using CarDealer.DTOs.Export.Cars;
using CarDealer.DTOs.Export.Suppliers;
using CarDealer.DTOs.Import.Cars;
using CarDealer.DTOs.Import.Customers;
using CarDealer.DTOs.Import.Parts;
using CarDealer.DTOs.Import.Sales;
using CarDealer.DTOs.Import.Suppliers;
using CarDealer.Models;
using CarDealer.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

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

            // Import

            // 09. Import Suppliers
            /*string suppliersXml = File.ReadAllText(@"../../../Datasets/suppliers.xml");
            string importSuppliersResult = ImportSuppliers(dbContext, suppliersXml);
            Console.WriteLine(importSuppliersResult);*/

            // 10. Import Parts
            /*string partsXml = File.ReadAllText(@"../../../Datasets/parts.xml");
            string importPartsResult = ImportParts(dbContext, partsXml);
            Console.WriteLine(importPartsResult);*/

            // 11. Import Cars
            /*string carsXml = File.ReadAllText(@"../../../Datasets/cars.xml");
            string importCarsResult = ImportCars(dbContext, carsXml);
            Console.WriteLine(importCarsResult);*/

            // 12. Import Customers
            /*string customersXml = File.ReadAllText(@"../../../Datasets/customers.xml");
            string importCustomersResult = ImportCustomers(dbContext, customersXml);
            Console.WriteLine(importCustomersResult);*/

            // 13. Import Sales
            /*string salesXml = File.ReadAllText(@"../../../Datasets/sales.xml");
            string importSalesResult = ImportSales(dbContext, salesXml);
            Console.WriteLine(importSalesResult);*/

            // Export

            // 14. Export Cars With Distance
            /*string exportCarsWithDistanceResult = GetCarsWithDistance(dbContext);
            Console.WriteLine(exportCarsWithDistanceResult);*/

            // 15. Export Cars From Make BMW
            /*string exportCarsFromMakeBMWResult = GetCarsFromMakeBmw(dbContext);
            Console.WriteLine(exportCarsFromMakeBMWResult);*/

            // 16. Export Local Suppliers
            /*string exportLocalSuppliersResult = GetLocalSuppliers(dbContext);
            Console.WriteLine(exportLocalSuppliersResult);*/

            // 17. Export Cars with Their List of Parts
            string exportCarsWithTheirListOfPartsResult = GetCarsWithTheirListOfParts(dbContext);
            Console.WriteLine(exportCarsWithTheirListOfPartsResult);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportSupplierDTO[] importSupplierDTOs
                 = xmlHelper.Deserialize<ImportSupplierDTO[]>(inputXml, "Suppliers");
            
            HashSet<Supplier> suppliersToImport = new HashSet<Supplier>();

            foreach (var supplerDto in importSupplierDTOs)
            {
                if (string.IsNullOrEmpty(supplerDto.Name))
                {
                    continue;
                }

                Supplier supplier = mapper.Map<Supplier>(supplerDto);
                suppliersToImport.Add(supplier);
            }

            context.Suppliers.AddRange(suppliersToImport);
            context.SaveChanges();

            return $"Successfully imported {suppliersToImport.Count}";
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportPartDTO[] importPartDTOs
                = xmlHelper.Deserialize<ImportPartDTO[]>(inputXml, "Parts");

            HashSet<Part> partsToImport = new HashSet<Part>();

            foreach (var partDto in importPartDTOs)
            {
                if (string.IsNullOrEmpty(partDto.Name))
                {
                    continue;
                }

                if (!partDto.SupplierId.HasValue
                    || !context.Suppliers.Any(s => s.Id == partDto.SupplierId))
                {
                    continue;
                }

                Part part = mapper.Map<Part>(partDto);
                partsToImport.Add(part);
            }

            context.Parts.AddRange(partsToImport);
            context.SaveChanges();

            return $"Successfully imported {partsToImport.Count}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCarDTO[] carDtos =
                xmlHelper.Deserialize<ImportCarDTO[]>(inputXml, "Cars");

            HashSet<Car> carsToImport = new HashSet<Car>();

            foreach (var carDto in carDtos)
            {
                if (string.IsNullOrEmpty(carDto.Make)
                    || string.IsNullOrEmpty(carDto.Model))
                {
                    continue;
                }

                Car car = mapper.Map<Car>(carDto);

                foreach (var partCarDto in carDto.Parts.DistinctBy(p => p.PartId))
                {
                    if (!context.Parts.Any(p => p.Id == partCarDto.PartId))
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        PartId = partCarDto.PartId
                    };

                    car.PartsCars.Add(partCar);
                }

                carsToImport.Add(car);
            }

            context.Cars.AddRange(carsToImport);
            context.SaveChanges();

            return $"Successfully imported {carsToImport.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCustomerDTO[] customerDtos =
                xmlHelper.Deserialize<ImportCustomerDTO[]>(inputXml, "Customers");

            HashSet<Customer> customersToImport = new HashSet<Customer>();

            foreach (var customerDto in customerDtos)
            {
                if (string.IsNullOrEmpty(customerDto.Name)
                    || string.IsNullOrEmpty(customerDto.BirthDate))
                {
                    continue;
                }

                Customer customer = mapper.Map<Customer>(customerDto);

                customersToImport.Add(customer);
            }

            context.Customers.AddRange(customersToImport);
            context.SaveChanges();

            return $"Successfully imported {customersToImport.Count}";
        }

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportSaleDTO[] saleDtos =
                xmlHelper.Deserialize<ImportSaleDTO[]>(inputXml, "Sales");

            int[] carIds = context.Cars
                .Select(c => c.Id)
                .ToArray();

            HashSet<Sale> salesToImport = new HashSet<Sale>();

            foreach (var saleDto in saleDtos)
            {
                if (!saleDto.CarId.HasValue
                    || !carIds.Any(id => id == saleDto.CarId.Value))
                {
                    continue;
                }

                Sale sale = mapper.Map<Sale>(saleDto);

                salesToImport.Add(sale);
            }

            context.Sales.AddRange(salesToImport);
            context.SaveChanges();

            return $"Successfully imported {salesToImport.Count}";
        }

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var carsWithDistanceToExport = context.Cars
                .AsNoTracking()
                .Where(c => c.TraveledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ProjectTo<ExportCarWithDistanceDTO>(mapper.ConfigurationProvider)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportCarWithDistanceDTO[]>(carsWithDistanceToExport, "cars");

            return result;
        }

        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var carsFromMakeBMWToExport = context.Cars
                .AsNoTracking()
                .Where(c => c.Make == "BMW")
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TraveledDistance)
                .ProjectTo<ExportCarFromMakeBMWDTO>(mapper.ConfigurationProvider)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportCarFromMakeBMWDTO[]>(carsFromMakeBMWToExport, "cars");

            return result;
        }

        public static string GetLocalSuppliers(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var localSuppliersToExport = context.Suppliers
                .AsNoTracking()
                .Where(s => s.IsImporter == false)
                .ProjectTo<ExportLocalSupplierDTO>(mapper.ConfigurationProvider)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportLocalSupplierDTO[]>(localSuppliersToExport, "suppliers");

            return result;
        }

        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            IMapper mapper = CreateMapper();

            var carsWithTheirListOfPartsToExport = context.Cars
                .AsNoTracking()
                .OrderByDescending(c => c.TraveledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ProjectTo<ExportCarWithItsListOfPartsDTO>(mapper.ConfigurationProvider)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportCarWithItsListOfPartsDTO[]>(carsWithTheirListOfPartsToExport, "cars");

            return result;
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