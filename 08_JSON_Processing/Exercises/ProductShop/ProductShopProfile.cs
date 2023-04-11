using AutoMapper;
using ProductShop.DTOs.Export.Categories;
using ProductShop.DTOs.Export.Products;
using ProductShop.DTOs.Import.Categories;
using ProductShop.DTOs.Import.CategoriesProducts;
using ProductShop.DTOs.Import.Products;
using ProductShop.DTOs.Import.Users;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            // Users
            this.CreateMap<ImportUserDTO, User>();

            // Products
            this.CreateMap<ImportProductDTO, Product>();
            this.CreateMap<Product, ExportProductInRangeDTO>()
                .ForMember(d => d.FullName, opt => opt.MapFrom(src => $"{src.Seller.FirstName} {src.Seller.LastName}"));

            // Categories
            this.CreateMap<ImportCategoryDTO, Category>();

            this.CreateMap<Category, ExportCategoryByProductsCountDTO>()
                .ForMember(d => d.ProductsCount, opt => opt.MapFrom(src => src.CategoriesProducts.Count))
                .ForMember(d => d.AveragePrice, opt => opt.MapFrom(src => Math.Round(src.CategoriesProducts.Average(cp => cp.Product.Price), 2)))
                .ForMember(d => d.TotalRevenue, opt => opt.MapFrom(src => Math.Round(src.CategoriesProducts.Sum(cp => cp.Product.Price), 2)));

            // CategoriesProducts
            this.CreateMap<ImportCategoryProductDTO, CategoryProduct>();
        }
    }
}
