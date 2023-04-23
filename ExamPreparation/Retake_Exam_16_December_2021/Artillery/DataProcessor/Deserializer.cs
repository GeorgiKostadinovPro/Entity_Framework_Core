namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto.Countries;
    using Artillery.DataProcessor.ImportDto.Guns;
    using Artillery.DataProcessor.ImportDto.Manufacturers;
    using Artillery.DataProcessor.ImportDto.Shells;
    using Artillery.Utilities;
    using AutoMapper;
    using Newtonsoft.Json;
    using System.ComponentModel.DataAnnotations;
    using System.Data.SqlTypes;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        private const string ErrorMessage =
            "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        private static XmlHelper xmlHelper;

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            xmlHelper = new XmlHelper();

            IMapper mapper = CreateMapper();

            ImportCountryDTO[] deserializedCountries
                = xmlHelper.Deserialize<ImportCountryDTO[]>(xmlString, "Countries");

            ICollection<Country> countriesToImport = new HashSet<Country>();

            foreach (var countryDto in deserializedCountries)
            {
                if (!IsValid(countryDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Country country = mapper.Map<Country>(countryDto);

                countriesToImport.Add(country);

                sb.AppendLine(string.Format(SuccessfulImportCountry, country.CountryName, country.ArmySize));
            }

            context.Countries.AddRange(countriesToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            xmlHelper = new XmlHelper();

            IMapper mapper = CreateMapper();

            ImportManufacturerDTO[] deserializedManufacturers
                = xmlHelper.Deserialize<ImportManufacturerDTO[]>(xmlString, "Manufacturers");

            foreach (var manufacturerDto in deserializedManufacturers)
            {
                if (!IsValid(manufacturerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                } 
                
                if (context.Manufacturers.Any(m => m.ManufacturerName.Contains(manufacturerDto.ManufacturerName)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                
                Manufacturer manufacturer = mapper.Map<Manufacturer>(manufacturerDto);

                context.Manufacturers.Add(manufacturer);
                context.SaveChanges();

                string[] foundedWholeData = manufacturerDto.Founded
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries);

                string foundedResult = $"{foundedWholeData[foundedWholeData.Length - 2]}, {foundedWholeData[foundedWholeData.Length - 1]}";

                sb.AppendLine(string.Format(SuccessfulImportManufacturer, manufacturer.ManufacturerName, foundedResult));
            }

            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            xmlHelper = new XmlHelper();

            IMapper mapper = CreateMapper();

            ImportShellDTO[] deserializedShells
                = xmlHelper.Deserialize<ImportShellDTO[]>(xmlString, "Shells");

            ICollection<Shell> shellsToImport = new HashSet<Shell>();

            foreach (var shellDto in deserializedShells)
            {
                if (!IsValid(shellDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Shell shell = mapper.Map<Shell>(shellDto);

                shellsToImport.Add(shell);

                sb.AppendLine(string.Format(SuccessfulImportShell, shell.Caliber, shell.ShellWeight));
            }

            context.Shells.AddRange(shellsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportGunDTO[] deserializedGuns
                = JsonConvert.DeserializeObject<ImportGunDTO[]>(jsonString);

            ICollection<Gun> gunsToImport = new HashSet<Gun>();

            foreach (var gunDto in deserializedGuns)
            {
                if (!IsValid(gunDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isValidGunType = Enum.TryParse<GunType>(gunDto.GunType, out GunType gunTypeResult);

                if (!isValidGunType)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Gun gun = new Gun
                {
                    GunWeight = gunDto.GunWeight,
                    BarrelLength = gunDto.BarrelLength,
                    NumberBuild = gunDto.NumberBuild,
                    Range = gunDto.Range,
                    GunType = gunTypeResult,
                    ManufacturerId = gunDto.ManufacturerId,
                    ShellId = gunDto.ShellId
                };

                foreach (var countryDto in gunDto.CountryIds)
                {
                    CountryGun countryGun = new CountryGun
                    {
                        Gun = gun,
                        CountryId = countryDto.Id
                    };

                    gun.CountriesGuns.Add(countryGun);
                }

                 gunsToImport.Add(gun);

                sb.AppendLine(string.Format(SuccessfulImportGun, gun.GunType.ToString(), gun.GunWeight, gun.BarrelLength));
            }

            context.Guns.AddRange(gunsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }

        private static IMapper CreateMapper()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ArtilleryProfile>();
            }));

            return mapper;
        }
    }
}