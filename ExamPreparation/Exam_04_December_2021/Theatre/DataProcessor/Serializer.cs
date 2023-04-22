namespace Theatre.DataProcessor
{
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Linq;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto.Casts;
    using Theatre.DataProcessor.ExportDto.Plays;
    using Theatre.DataProcessor.ExportDto.Theatres;
    using Theatre.DataProcessor.ExportDto.Tickets;
    using Theatre.Utilities;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatresToExport = context.Theatres
                .ToArray()
                .Where(t => t.NumberOfHalls >= numbersOfHalls
                && t.Tickets.Count >= 20)
                .Select(t => new ExportTheatreDTO
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                                   .Where(ticket => ticket.RowNumber >= 1
                                   && ticket.RowNumber <= 5)
                                   .Sum(ticket => ticket.Price),
                    Tickets = t.Tickets
                               .Where(ticket => ticket.RowNumber >= 1
                               && ticket.RowNumber <= 5)
                               .Select(ticket => new ExportTicketDTO
                               {
                                   Price = ticket.Price,
                                   RowNumber = ticket.RowNumber
                               })
                               .OrderByDescending(t => t.Price)
                               .ToArray()
                })
                .OrderByDescending(t =>  t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            string result = JsonConvert.SerializeObject(theatresToExport, Formatting.Indented);

            return result;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var playsToExport = context.Plays
                .ToArray()
                .Where(p => p.Rating <= rating)
                .Select(p => new ExportPlayDTO
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating > 0 ? p.Rating.ToString() : "Premier",
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                              .Where(c => c.IsMainCharacter == true)
                              .Select(c => new ExportCastDTO
                              {
                                  FullName = c.FullName,
                                  MainCharacter = $"Plays main character in '{p.Title}'."
                              })
                              .OrderByDescending(c => c.FullName)
                              .ToArray()
                })
                .OrderBy(p => p.Title)
                .ThenByDescending(p => p.Genre)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportPlayDTO[]>(playsToExport, "Plays");

            return result;
        }
    }
}
