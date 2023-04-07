namespace MusicHub
{
    using System;
    using System.Globalization;
    using System.Text;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;

    public class StartUp
    {
        public static void Main()
        {
            using MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            //Test your solutions here
            /*string allAlbumsProducedByGivenProducer = ExportAlbumsInfo(context, 9);
            Console.WriteLine(allAlbumsProducedByGivenProducer);*/

            string allSongsAboveGivenDuration = ExportSongsAboveDuration(context, 4);
            Console.WriteLine(allSongsAboveGivenDuration);
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            StringBuilder sb = new StringBuilder();

            var albums = context.Albums
                .Where(a => a.ProducerId.HasValue &&
                            a.ProducerId == producerId)
                .ToArray()
                .Select(a => new
                {
                    AlbumName = a.Name,
                    ReleaseDate = a.ReleaseDate.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs
                             .Select(s => new
                             {
                                 SongName = s.Name,
                                 s.Price,
                                 Writer = s.Writer.Name
                             })
                             .OrderByDescending(s => s.SongName)
                             .ThenBy(s => s.Writer)
                             .ToArray(),

                    TotalAlbumPrice = a.Price
                })
                .OrderByDescending(a => a.TotalAlbumPrice)
                .ToArray();

            foreach (var album in albums)
            {
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.ReleaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");

                int i = 1;

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{i}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.Price:f2}");
                    sb.AppendLine($"---Writer: {song.Writer}");

                    i++;
                }

                sb.AppendLine($"-AlbumPrice: {album.TotalAlbumPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        {
            StringBuilder sb = new StringBuilder();

            var songs = context.Songs
                .ToArray()
                .Where(s => s.Duration.TotalSeconds > duration)
                .Select(s => new
                {
                    SongName = s.Name,
                    Writer = s.Writer.Name,
                    Performers = s.SongPerformers
                                 .Select(sp => new { 
                                     PerformerFullName = sp.Performer.FirstName + ' ' + sp.Performer.LastName
                                 })
                                 .OrderBy(p => p.PerformerFullName)
                                 .ToArray(),

                    AlbumProducer = s.Album.Producer.Name,
                    Duration = s.Duration
                })
                .OrderBy(s => s.SongName)
                .ThenBy(s => s.Writer)
                .ToArray();

            int i = 1;

            foreach (var song in songs)
            {
                sb.AppendLine($"-Song #{i}");
                sb.AppendLine($"---SongName: {song.SongName}");
                sb.AppendLine($"---Writer: {song.Writer}");

                foreach (var performer in song.Performers)
                {
                    sb.AppendLine($"---Performer: {performer.PerformerFullName}");
                }    
                
                sb.AppendLine($"---AlbumProducer: {song.AlbumProducer}");
                sb.AppendLine($"---Duration: {song.Duration:c}");

                i++;
            }

            return sb.ToString().TrimEnd();
        }
    }
}