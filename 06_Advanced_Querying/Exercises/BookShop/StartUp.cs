namespace BookShop
{
    using BookShop.Models.Enums;
    using Data;
    using Initializer;
    using Microsoft.EntityFrameworkCore;
    using System.Globalization;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            // 01. Create the BookShop Database
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            // 02. Age Restriction
            /*string ageRestriction = Console.ReadLine();

            string booksByAgeRestriction = GetBooksByAgeRestriction(db, ageRestriction);
            Console.WriteLine(booksByAgeRestriction);*/

            // 03. Golden Books
            /*string goldenBooks = GetGoldenBooks(db);
            Console.WriteLine(goldenBooks);*/

            // 04. Books by Price
            /*string booksByPrice = GetBooksByPrice(db);
            Console.WriteLine(booksByPrice);*/

            // 05. Not Released In
            /*int year = int.Parse(Console.ReadLine());

            string booksNotReleasedIn = GetBooksNotReleasedIn(db, year);
            Console.WriteLine(booksNotReleasedIn);*/

            // 06. Book Titles by Category
            /*string category = Console.ReadLine();

            string booksTitlesByCategory = GetBooksByCategory(db, category);
            Console.WriteLine(booksTitlesByCategory);*/

            // 07. Released Before Date
            /*string date = Console.ReadLine();

            string booksReleasedBeforeDate = GetBooksReleasedBefore(db, date);
            Console.WriteLine(booksReleasedBeforeDate);*/

            // 08. Author Search
            /*string input = Console.ReadLine();

            string authorNamesEndingIn = GetAuthorNamesEndingIn(db, input);
            Console.WriteLine(authorNamesEndingIn);*/

            // 09. Book Search
            /*string input = Console.ReadLine();

            string bookTitlesContaining = GetBookTitlesContaining(db, input);
            Console.WriteLine(bookTitlesContaining);*/

            // 10. Book Search by Author
            /*string input = Console.ReadLine();

            string booksByAuthor = GetBooksByAuthor(db, input);
            Console.WriteLine(booksByAuthor);*/

            // 11. Count Books
            /*int lengthCheck = int.Parse(Console.ReadLine());

            int bookCount = CountBooks(db, lengthCheck);
            Console.WriteLine(bookCount);*/

            // 12. Total Book Copies
            /*string totalBookCopies = CountCopiesByAuthor(db);
            Console.WriteLine(totalBookCopies);*/

            // 13. Profit by Category
            /*string booksTotalProfitByCategory = GetTotalProfitByCategory(db);
            Console.WriteLine(booksTotalProfitByCategory);*/

            // 14. Most Recent Books
            /*string mostRecentBooksByCategories = GetMostRecentBooks(db);
            Console.WriteLine(mostRecentBooksByCategories);*/

            // 15. Increase Prices
            // This is an update query. No output!

            // 16. Remove Books
            int removedBooksCount = RemoveBooks(db);
            Console.WriteLine(removedBooksCount);
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

                var bookByAgeRestriction = context.Books
                    .AsNoTracking()
                    .Where(b => b.AgeRestriction == ageRestriction)
                    .Select(b => new
                    {
                       BookTitle = b.Title
                    })
                    .OrderBy(b => b.BookTitle)
                    .ToArray();

                foreach (var book in bookByAgeRestriction)
                {
                    sb.AppendLine(book.BookTitle);
                }
                
                return sb.ToString().TrimEnd();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var goldenBooks = context.Books
                    .AsNoTracking()
                    .Where(b => b.EditionType == EditionType.Gold &&
                                b.Copies < 5000)
                    .OrderBy(b => b.BookId)
                    .Select(b => new
                    {
                        BookTitle = b.Title
                    })
                    .ToArray();

            foreach (var book in goldenBooks)
            {
                sb.AppendLine(book.BookTitle);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var booksByPrice = context.Books
                .AsNoTracking()
                .Where(b => b.Price > 40)
                .Select(b => new
                {
                    BookTitle = b.Title,
                    BookPrice = b.Price
                })
                .OrderByDescending(b => b.BookPrice)
                .ToArray();

            foreach (var book in booksByPrice)
            {
                sb.AppendLine($"{book.BookTitle} - ${book.BookPrice:f2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            StringBuilder sb = new StringBuilder();

            var booksNotReleasedIn = context.Books
                .AsNoTracking()
                .Where(b => b.ReleaseDate!.Value.Year != year)
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    BookTitle = b.Title
                })
                .ToArray();

            foreach (var book in booksNotReleasedIn)
            {
                sb.AppendLine(book.BookTitle);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            var booksByCategory = context.Books
                .AsNoTracking()
                .Where(b => b.BookCategories.Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => new
                { 
                   b.Title
                })
                .ToArray();

            foreach (var book in booksByCategory)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            StringBuilder sb = new StringBuilder();

            var parsedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            var booksReleasedBefore = context.Books
                .AsNoTracking()
                .Where(b => b.ReleaseDate < parsedDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(b => new
                {
                    b.Title,
                    b.EditionType,
                    b.Price
                })
                .ToArray();

            foreach (var book in booksReleasedBefore)
            {
                sb.AppendLine($"{book.Title} - {book.EditionType.ToString()} - ${book.Price:f2}");
            }
                
            return sb.ToString().TrimEnd();
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var authorNamesEndingIn = context.Authors
                .AsNoTracking()
                .Where(a => a.FirstName.EndsWith(input))
                .Select(a => new
                {
                    FullName = a.FirstName + " " + a.LastName
                })
                .OrderBy(a => a.FullName)
                .ToArray();

            foreach (var author in authorNamesEndingIn)
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var bookTitlesContaining = context.Books
                .AsNoTracking()
                .Where(b => b.Title.ToLower().Contains(input.ToLower()))
                .Select(b => new
                {
                    b.Title,
                })
                .OrderBy(b => b.Title)
                .ToArray();

            foreach (var book in bookTitlesContaining)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            StringBuilder sb = new StringBuilder();

            var booksByAuthor = context.Books
                .AsNoTracking()
                .Where(b => b.Author.LastName.ToLower().StartsWith(input.ToLower()))
                .OrderBy(b => b.BookId)
                .Select(b => new
                {
                    b.Title,
                    AuthorFullName = b.Author.FirstName + " " + b.Author.LastName
                })
                .ToArray();

            foreach (var book in booksByAuthor)
            {
                sb.AppendLine($"{book.Title} ({book.AuthorFullName})");
            }

            return sb.ToString().TrimEnd();
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            int countBooks = context.Books
                .Count(b => b.Title.Length > lengthCheck);

            return countBooks;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var totalBookCopies = context.Authors
                .AsNoTracking()
                .Select(a => new
                {
                    AuthorFullName = a.FirstName + " " + a.LastName,
                    TotalBookCopies = a.Books.Sum(b => b.Copies)
                })
                .OrderByDescending(a => a.TotalBookCopies)
                .ToArray();

            foreach (var bookCopies in totalBookCopies)
            {
                sb.AppendLine($"{bookCopies.AuthorFullName} - {bookCopies.TotalBookCopies}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var booksTotalProfitByCategory = context.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    BooksTotalProfit = c.CategoryBooks
                                        .Select(cb => cb.Book)
                                        .Sum(b => b.Copies * b.Price)
                })
                .OrderByDescending(c => c.BooksTotalProfit)
                .ThenBy(c => c.Name)
                .ToArray();

            foreach (var category in booksTotalProfitByCategory)
            {
                sb.AppendLine($"{category.Name} ${category.BooksTotalProfit:f2}");
            }


            return sb.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var mostRecentBooksByCategories = context.Categories
                .AsNoTracking()
                .Select(c => new
                {
                    c.Name,
                    CategoryMostRecentBooks = c.CategoryBooks
                                               .Select(cb => cb.Book)
                                               .OrderByDescending(b => b.ReleaseDate)
                                               .Take(3)
                                               .ToArray()
                })
                .OrderBy(c => c.Name)
                .ToArray();

            foreach (var category in mostRecentBooksByCategories)
            {
                sb.AppendLine($"--{category.Name}");

                foreach (var book in category.CategoryMostRecentBooks)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate!.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var booksToUpdate = context.Books
                .Where(b => b.ReleaseDate!.Value.Year < 2010)
                .ToArray();

            foreach (var book in booksToUpdate)
            {
                book.Price += 5;
            }

            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var booksToRemove = context.Books
                .Where(b => b.Copies < 4200)
                .ToArray();

            var booksCategoriesToRemove = context.BooksCategories
                .Where(bc => bc.Book.Copies < 4200)
                .ToArray();

            if (booksCategoriesToRemove.Any())
            {
                foreach (var bookCategory in booksCategoriesToRemove)
                {
                    context.BooksCategories.Remove(bookCategory);
                }
            }

            foreach (var book in booksToRemove)
            {
                context.Books.Remove(book);
            }

            context.SaveChanges();

            return booksToRemove.Length;
        }
    }
}