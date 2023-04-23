namespace Artillery
{
    using Artillery.Data.Models;
    using Artillery.DataProcessor.ImportDto.Countries;
    using Artillery.DataProcessor.ImportDto.Manufacturers;
    using Artillery.DataProcessor.ImportDto.Shells;
    using AutoMapper;

    public class ArtilleryProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public ArtilleryProfile()
        {
            // Country
            this.CreateMap<ImportCountryDTO, Country>();

            // Manufacturer
            this.CreateMap<ImportManufacturerDTO, Manufacturer>();

            // Shell
            this.CreateMap<ImportShellDTO, Shell>();
        }
    }
}