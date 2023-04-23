
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.DataProcessor.ExportDto.Countires;
    using Artillery.DataProcessor.ExportDto.Guns;
    using Artillery.DataProcessor.ExportDto.Shells;
    using Artillery.Utilities;
    using Newtonsoft.Json;
    using System.Linq;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shellsToExport = context.Shells
                .ToArray()
                .Where(s => s.ShellWeight > shellWeight)
                .Select(s => new ExportShellWithGunsDTO
                {
                    ShellWeight = s.ShellWeight,
                    Caliber = s.Caliber,
                    Guns = s.Guns
                            .Where(g => g.GunType.ToString() == "AntiAircraftGun")
                            .Select(g => new ExportSingleGunDTO
                            {
                                GunType = g.GunType.ToString(),
                                GunWeight = g.GunWeight,
                                BarrelLength = g.BarrelLength,
                                Range = g.Range > 3000 ? "Long-range" : "Regular range"
                            })
                            .OrderByDescending(g => g.GunWeight)
                            .ToArray()
                })
                .OrderBy(s => s.ShellWeight)
                .ToArray();

            string result = JsonConvert.SerializeObject(shellsToExport, Formatting.Indented);

            return result;
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var gunsToExport = context.Guns
                .ToArray()
                .Where(g => g.Manufacturer.ManufacturerName == manufacturer)
                .Select(g => new ExportGunWithCountriesDTO
                {
                    Manufacturer = g.Manufacturer.ManufacturerName,
                    GunType = g.GunType.ToString(),
                    GunWeight = g.GunWeight,
                    BarrelLength = g.BarrelLength,
                    Range = g.Range,
                    Countries = g.CountriesGuns
                                 .Where(cg => cg.Country.ArmySize > 4500000)
                                 .Select(cg => new ExportCountryDTO
                                 {
                                     Country = cg.Country.CountryName,
                                     ArmySize = cg.Country.ArmySize
                                 })
                                 .OrderBy(c => c.ArmySize)
                                 .ToArray()
                })
                .OrderBy(g => g.BarrelLength)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportGunWithCountriesDTO[]>(gunsToExport, "Guns");

            return result;
        }
    }
}
