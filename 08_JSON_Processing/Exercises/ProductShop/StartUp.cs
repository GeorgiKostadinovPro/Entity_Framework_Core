using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export.Categories;
using ProductShop.DTOs.Export.Products;
using ProductShop.DTOs.Import.Categories;
using ProductShop.DTOs.Import.CategoriesProducts;
using ProductShop.DTOs.Import.Products;
using ProductShop.DTOs.Import.Users;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        //private static IMapper mapper;

        public static void Main()
        {
            // Judge does not accept this Automapper! It accepts only the static one.
            // The method CreateMapper() will create an instance of it and it will be used in all methods. 

            /*mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));*/

            using ProductShopContext dbContext = new ProductShopContext();

            /*dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();*/

            // Import Data

            // 01. Import Users
            /*string usersJson = File.ReadAllText(@"../../../Datasets/users.json");
            string importUsersResult = ImportUsers(dbContext, usersJson);
            Console.WriteLine(importUsersResult);*/

            // 02. Import Products
            /*string productsJson = File.ReadAllText(@"../../../Datasets/products.json");
            string importProductsResult = ImportProducts(dbContext, productsJson);
            Console.WriteLine(importProductsResult);*/

            // 03. Import Categories
            /*string categoriesJson = File.ReadAllText(@"../../../Datasets/categories.json");
            string importCategoriesResult = ImportCategories(dbContext, categoriesJson);
            Console.WriteLine(importCategoriesResult);*/

            // 04. Import Categories and Products
            /*string categoriesAndProductsJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
            string importCategoriesAndProductsResult = ImportCategoryProducts(dbContext, categoriesAndProductsJson);
            Console.WriteLine(importCategoriesAndProductsResult);*/

            // Export

            // 05. Export Products in Range
            /*string exportProductsInRangeResult = GetProductsInRange(dbContext);
            Console.WriteLine(exportProductsInRangeResult);*/

            // 06. Export Sold Products
            /*string exportSoldProductsResult = GetSoldProducts(dbContext);
            Console.WriteLine(exportSoldProductsResult);*/

            // 07.Export Categories By Products Count
            /*string exportCategoriesByProductsCountResult = GetCategoriesByProductsCount(dbContext);
            Console.WriteLine(exportCategoriesByProductsCountResult);*/

            // 8. Export Users and Products
            string exportUsersAndProductsResult = GetUsersWithProducts(dbContext);
            Console.WriteLine(exportUsersAndProductsResult);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            IEnumerable<ImportUserDTO> desirializedUsers
                = JsonConvert.DeserializeObject<IEnumerable<ImportUserDTO>>(inputJson);

            HashSet<User> usersToImport = mapper.Map<HashSet<User>>(desirializedUsers);

            context.Users.AddRange(usersToImport);
            context.SaveChanges();

            return $"Successfully imported {usersToImport.Count}";
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            IEnumerable <ImportProductDTO> desirializedProducts =
                JsonConvert.DeserializeObject<IEnumerable<ImportProductDTO>>(inputJson);

            HashSet<Product> productsToImport = mapper.Map<HashSet<Product>>(desirializedProducts);

            context.Products.AddRange(productsToImport);
            context.SaveChanges();

            return $"Successfully imported {productsToImport.Count}";
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            IEnumerable<ImportCategoryDTO> desirializedCategories =
               JsonConvert.DeserializeObject<IEnumerable<ImportCategoryDTO>>(inputJson);

            HashSet<Category> categoriesToImport = new HashSet<Category>();

            foreach (var categoryToImport in desirializedCategories)
            {
                if (categoryToImport.Name == null)
                {
                    continue;
                }

                Category category = mapper.Map<Category>(categoryToImport);

                categoriesToImport.Add(category);
            }

            context.Categories.AddRange(categoriesToImport);
            context.SaveChanges();

            return $"Successfully imported {categoriesToImport.Count}";
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IMapper mapper = CreateMapper();

            IEnumerable<ImportCategoryProductDTO> desirializedCategoriesProducts =
                JsonConvert.DeserializeObject<IEnumerable<ImportCategoryProductDTO>>(inputJson);

            HashSet<CategoryProduct> categoriesProductsToImport = mapper.Map<HashSet<CategoryProduct>>(desirializedCategoriesProducts);

            context.CategoriesProducts.AddRange(categoriesProductsToImport);
            context.SaveChanges();

            return $"Successfully imported {categoriesProductsToImport.Count}";
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            IMapper mapper = CreateMapper();

            ExportProductInRangeDTO[] productsInRange = context.Products
                .AsNoTracking()
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ExportProductInRangeDTO>(mapper.ConfigurationProvider)
                .ToArray();

            return JsonConvert.SerializeObject(productsInRange, Formatting.Indented);
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersSoldProducts = context.Users
                .AsNoTracking()
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    SoldProducts = u.ProductsSold
                                    .Where(p => p.Buyer != null)
                                    .Select(p => new
                                    {
                                        p.Name,
                                        p.Price,
                                        BuyerFirstName = p.Buyer.FirstName,
                                        BuyerLastName = p.Buyer.LastName
                                    })
                                    .ToArray()
                })
                .ToArray();

            string exportResult = JsonConvert.SerializeObject(usersSoldProducts, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = ConfigureCamelCaseNaming()
            });

            return exportResult;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            /*IMapper mapper = CreateMapper();

            ExportCategoryByProductsCountDTO[] categoriesToExport = context.Categories
                .AsNoTracking()
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .ProjectTo<ExportCategoryByProductsCountDTO>(mapper.ConfigurationProvider)
                .ToArray();*/

            var categoriesByProductsCountToExport = context.Categories
                .OrderByDescending(c => c.CategoriesProducts.Count)
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoriesProducts.Count,
                    averagePrice = Math.Round((double)c.CategoriesProducts.Average(p => p.Product.Price), 2),
                    totalRevenue = Math.Round((double)c.CategoriesProducts.Sum(p => p.Product.Price), 2)
                })
                .ToArray();

            return JsonConvert.SerializeObject(categoriesByProductsCountToExport, Formatting.Indented);
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {
            IContractResolver contractResolver = ConfigureCamelCaseNaming();

            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    // UserDTO
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        // ProductWrapperDTO
                        Count = u.ProductsSold
                            .Count(p => p.Buyer != null),
                        Products = u.ProductsSold
                            .Where(p => p.Buyer != null)
                            .Select(p => new
                            {
                                // ProductDTO
                                p.Name,
                                p.Price
                            })
                            .ToArray()
                    }
                })
                .OrderByDescending(u => u.SoldProducts.Count)
                .AsNoTracking()
                .ToArray();

            var userWrapperDto = new
            {
                UsersCount = users.Length,
                Users = users
            };

            return JsonConvert.SerializeObject(userWrapperDto,
                Formatting.Indented,
                new JsonSerializerSettings()
                {
                    ContractResolver = contractResolver,
                    NullValueHandling = NullValueHandling.Ignore
                });
        }

        private static IMapper CreateMapper()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            }));

            return mapper;
        }

        private static IContractResolver ConfigureCamelCaseNaming()
        {
            DefaultContractResolver defaultContractResolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
                {
                    OverrideSpecifiedNames = false
                }
            };

            return defaultContractResolver;
        }
    }
}