using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs.Export.Categories;
using ProductShop.DTOs.Export.Products;
using ProductShop.DTOs.Export.Users;
using ProductShop.DTOs.Import.Categories;
using ProductShop.DTOs.Import.CategoriesProducts;
using ProductShop.DTOs.Import.Products;
using ProductShop.DTOs.Import.Users;
using ProductShop.Models;
using ProductShop.Utilities;
using System.Linq;
using System.Xml;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            // 01. Setup Database
            using ProductShopContext dbContext = new ProductShopContext();

            /*dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();*/

            // Import Data

            // 01. Import Users
            /*string usersXml = File.ReadAllText(@"../../../Datasets/users.xml");
            string importUsersResult = ImportUsers(dbContext, usersXml);
            Console.WriteLine(importUsersResult);*/

            // 02. Import Products
            /*string productsXml = File.ReadAllText(@"../../../Datasets/products.xml");
            string importProductsResult = ImportProducts(dbContext, productsXml);
            Console.WriteLine(importProductsResult);*/

            // 03. Import Categories
            /*string categoriesXml = File.ReadAllText(@"../../../Datasets/categories.xml");
            string importCategoriesResult = ImportCategories(dbContext, categoriesXml);
            Console.WriteLine(importCategoriesResult);*/

            // 04. Import Categories and Products
            /*string categoriesAndProductsXml = File.ReadAllText(@"../../../Datasets/categories-products.xml");
            string importCategoriesAndProductsResult = ImportCategoryProducts(dbContext, categoriesAndProductsXml);
            Console.WriteLine(importCategoriesAndProductsResult);*/

            // Export

            // 05. Export Products in Range
            /*string exportProductsInRangeResult = GetProductsInRange(dbContext);
            Console.WriteLine(exportProductsInRangeResult);*/

            // 06. Export Sold Products
            /*string exportSoldProductsResult = GetSoldProducts(dbContext);
            Console.WriteLine(exportSoldProductsResult);*/

            // 07. Export Categories By Products Count
            /*string exportCategoriesByProductsCountResult = GetCategoriesByProductsCount(dbContext);
            Console.WriteLine(exportCategoriesByProductsCountResult);*/

            // 8. Export Users and Products
            string exportUsersAndProductsResult = GetUsersWithProducts(dbContext);
            Console.WriteLine(exportUsersAndProductsResult);
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportUserDTO[] userDtos
                = xmlHelper.Deserialize<ImportUserDTO[]>(inputXml, "Users");

            HashSet<User> usersToImport = new HashSet<User>();

            foreach (var userToImport in userDtos)
            {
                if (!userToImport.Age.HasValue)
                {
                    continue;
                }

                User user = mapper.Map<User>(userToImport);
                
                usersToImport.Add(user);
            }

            context.Users.AddRange(usersToImport);
            context.SaveChanges();

            return $"Successfully imported {usersToImport.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportProductDTO[] productDtos
                = xmlHelper.Deserialize<ImportProductDTO[]>(inputXml, "Products");

            HashSet<Product> productsToImport = mapper.Map<HashSet<Product>>(productDtos);

            context.Products.AddRange(productsToImport);
            context.SaveChanges();

            return $"Successfully imported {productsToImport.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCategoryDTO[] categoryDtos
              = xmlHelper.Deserialize<ImportCategoryDTO[]>(inputXml, "Categories");

            HashSet<Category> categoriesToImport = mapper.Map<HashSet<Category>>(categoryDtos);

            context.Categories.AddRange(categoriesToImport);
            context.SaveChanges();

            return $"Successfully imported {categoriesToImport.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            IMapper mapper = CreateMapper();

            XmlHelper xmlHelper = new XmlHelper();

            ImportCategoryProductDTO[] categoryProductsDtos
              = xmlHelper.Deserialize<ImportCategoryProductDTO[]>(inputXml, "CategoryProducts");

            HashSet<CategoryProduct> categoryProductsToImport = mapper.Map<HashSet<CategoryProduct>>(categoryProductsDtos);

            context.CategoryProducts.AddRange(categoryProductsToImport);
            context.SaveChanges();

            return $"Successfully imported {categoryProductsToImport.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var productsInRangeToExport = context.Products
                .AsNoTracking()
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Take(10)
                .ProjectTo<ExportProductInRange>(mapper.ConfigurationProvider)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportProductInRange[]>(productsInRangeToExport, "Products");

            return result;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var soldProductsToExport = context.Users
                .AsNoTracking()
                .Where(u => u.ProductsSold.Any())
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Take(5)
                .ProjectTo<ExportUserWithSoldProductsDTO>(mapper.ConfigurationProvider)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportUserWithSoldProductsDTO[]>(soldProductsToExport, "Users");

            return result;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            var categoriesByProductsCountToExport = context.Categories
                .AsNoTracking()
                .ProjectTo<ExportCategoryByProductsCountDTO>(mapper.ConfigurationProvider)
                .OrderByDescending(u => u.Count)
                .ThenBy(u => u.TotalRevenue)
                .ToArray();

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportCategoryByProductsCountDTO[]>(categoriesByProductsCountToExport, "Categories");

            return result;
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersAndCategoriesToExport = context.Users
                .AsNoTracking()
                .Where(u => u.ProductsSold.Any())
                .Select(u => new ExportUserAndProductsDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportSoldProductsCountAndInfoDTO
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                                    .Select(p => new ExportProductDTO
                                    {
                                        Name = p.Name,
                                        Price = p.Price
                                    })
                                    .OrderByDescending(p => p.Price)
                                    .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .Take(10)
                .ToArray();

            ExportUsersGeneralInfoDTO usersInfo = new ExportUsersGeneralInfoDTO
            {
                UsersCount = context.Users.Count(u => u.ProductsSold.Any()),
                Users = usersAndCategoriesToExport
            };

            XmlHelper xmlHelper = new XmlHelper();

            string result = xmlHelper.Serialize<ExportUsersGeneralInfoDTO>(usersInfo, "Users");

            return result;
        }

        private static IMapper CreateMapper()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            return mapper;
        }
    }
}