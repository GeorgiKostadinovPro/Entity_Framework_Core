namespace Theatre.DataProcessor
{
    using Castle.DynamicProxy.Contributors;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto.Cars;
    using Theatre.DataProcessor.ImportDto.Plays;
    using Theatre.DataProcessor.ImportDto.Theatres;
    using Theatre.Utilities;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        private static XmlHelper xmlHelper;

        public static string ImportPlays(TheatreContext context, string xmlString)
        {   
            StringBuilder sb = new StringBuilder();

            xmlHelper = new XmlHelper();

            ImportPlayDTO[] deserializedPlays
                = xmlHelper.Deserialize<ImportPlayDTO[]>(xmlString, "Plays");

            ICollection<Play> playsToImport = new HashSet<Play>();

            foreach (var playDto in deserializedPlays)
            {
                if (!IsValid(playDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                TimeSpan duration = TimeSpan.Parse(playDto.Duration, CultureInfo.InvariantCulture);

                if (duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isGenreValid = Enum.TryParse<Genre>(playDto.Genre, out Genre genreResult);

                if (!isGenreValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Genre genre = genreResult;

                Play play = new Play
                {
                    Title = playDto.Title,
                    Duration = duration,
                    Rating = playDto.Rating,
                    Genre = genre,
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter
                };

                playsToImport.Add(play);

                sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre.ToString(), play.Rating));
            }

            context.Plays.AddRange(playsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            xmlHelper = new XmlHelper();

            ImportCastDTO[] deserializedCasts
                = xmlHelper.Deserialize<ImportCastDTO[]>(xmlString, "Casts");

            ICollection<Cast> castsToImport = new HashSet<Cast>();

            //int[] validPlayIds = context.Plays
            //    .Select(p => p.Id)
            //    .ToArray();

            foreach (var castDto in deserializedCasts)
            {
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //if (!validPlayIds.Contains(castDto.PlayId))
                //{
                //    sb.AppendLine(ErrorMessage);
                //    continue;
                //}

                Cast cast = new Cast
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };

                castsToImport.Add(cast);

                string isMainOrLesserCharacter = cast.IsMainCharacter ? "main" : "lesser";

                sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName, isMainOrLesserCharacter));
            }

            context.Casts.AddRange(castsToImport);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            ImportTheatreDTO[] deserializedTheatres
                = JsonConvert.DeserializeObject<ImportTheatreDTO[]>(jsonString);

            ICollection<Theatre> theatresToImport = new HashSet<Theatre>();

            foreach (var theatreDto in deserializedTheatres)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director
                };

                foreach (var ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId
                    };

                    theatre.Tickets.Add(ticket);
                }

                theatresToImport.Add(theatre);

                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(theatresToImport);
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
    }
}
