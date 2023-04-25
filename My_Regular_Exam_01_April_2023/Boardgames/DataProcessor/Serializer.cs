namespace Boardgames.DataProcessor
{
    using Boardgames.Data;
    using Boardgames.DataProcessor.ExportDto.Boardgames;
    using Boardgames.DataProcessor.ExportDto.Creators;
    using Boardgames.DataProcessor.ExportDto.Sellers;
    using Boardgames.Utilities;
    using Newtonsoft.Json;
    using System.Linq;

    public class Serializer
    {
        private static XmlHelper xmlHelper;

        public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
        {
            var creatorsToExport = context.Creators
                .ToArray()
                .Where(c => c.Boardgames.Count > 0)
                .Select(c => new ExportCreatorWithBoardgamesDTO
                {
                    BoardgamesCount = c.Boardgames.Count,
                    CreatorName = $"{c.FirstName} {c.LastName}",
                    Boardgames = c.Boardgames
                                  .Select(b => new ExportBoardgameForCreatorDTO
                                  {
                                      BoardgameName = b.Name,
                                      BoardgameYearPublished = b.YearPublished
                                  })
                                  .OrderBy(b => b.BoardgameName)
                                  .ToArray()
                })
                .OrderByDescending(c => c.BoardgamesCount)
                .ThenBy(c => c.CreatorName)
                .ToArray();

            xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportCreatorWithBoardgamesDTO[]>(creatorsToExport, "Creators");

            return result;
        }

        public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
        {
            var sellersToExport = context.Sellers
                .ToArray()
                .Where(s => s.BoardgamesSellers.Any(bs => bs.Boardgame.YearPublished >= year
                && bs.Boardgame.Rating <= rating))
                .Select(s => new ExportSellerWithBoardgamesDTO
                {
                    Name = s.Name,
                    Website = s.Website,
                    Boardgames = s.BoardgamesSellers.Where(bs => bs.Boardgame.YearPublished >= year
                                 && bs.Boardgame.Rating <= rating)
                                  .Select(bs => new ExportBoardgameForSellerDTO
                                  {
                                      Name = bs.Boardgame.Name,
                                      Rating = bs.Boardgame.Rating,
                                      Mechanics = bs.Boardgame.Mechanics,
                                      Category = bs.Boardgame.CategoryType.ToString()
                                  })
                                  .OrderByDescending(b => b.Rating)
                                  .ThenBy(b => b.Name)
                                  .ToArray()
                })
                .OrderByDescending(s => s.Boardgames.Length)
                .ThenBy(s => s.Name)
                .Take(5)
                .ToArray();

            string result = JsonConvert.SerializeObject(sellersToExport, Formatting.Indented);

            return result;
        }
    }
}