using AutoMapper;
using Microsoft.Data.SqlClient;
using ProductShop.DTOs.Export.Categories;
using ProductShop.DTOs.Export.Products;
using ProductShop.DTOs.Export.Users;
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
            this.CreateMap<ImportUserDTO, User>()
                .ForMember(d => d.Age, opt => opt.MapFrom(src => src.Age!.Value));

            this.CreateMap<User, ExportUserWithSoldProductsDTO>()
                .ForMember(d => d.SoldProducts, opt => opt.MapFrom(src => src.ProductsSold));

            // Products
            this.CreateMap<ImportProductDTO, Product>();

            this.CreateMap<Product, ExportProductInRange>()
                .ForMember(d => d.Buyer, opt => opt.MapFrom(src => $"{src.Buyer!.FirstName} {src.Buyer!.LastName}"));

            this.CreateMap<Product, ExportProductDTO>();

            // Categories
            this.CreateMap<ImportCategoryDTO, Category>();

            this.CreateMap<Category, ExportCategoryByProductsCountDTO>()
                .ForMember(d => d.Count, opt => opt.MapFrom(src => src.CategoryProducts.Count))
                .ForMember(d => d.AveragePrice, opt => opt.MapFrom(src => src.CategoryProducts.Average(cp => cp.Product.Price)))
                .ForMember(d => d.TotalRevenue, opt => opt.MapFrom(src => src.CategoryProducts.Sum(cp => cp.Product.Price)));

            // CategoriesProducts
            this.CreateMap<ImportCategoryProductDTO, CategoryProduct>();
        }
    }
}