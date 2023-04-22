namespace Theatre
{
    using AutoMapper;
    using System;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto.Plays;

    public class TheatreProfile : Profile
    {
        // Configure your AutoMapper here if you wish to use it. If not, DO NOT DELETE THIS CLASS
        public TheatreProfile()
        {
            // Plays
            this.CreateMap<ImportPlayDTO, Play>()
                .ForMember(d => d.Duration, src => src.MapFrom(src => TimeSpan.Parse(src.Duration)))
                .ForSourceMember(src => src.Genre, opt => opt.DoNotValidate());
        }
    }
}
