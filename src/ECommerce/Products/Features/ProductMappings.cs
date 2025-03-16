
using AutoMapper;
using BuildingBlocks.Core.Pagination;
using ECommerce.Infrastructure.Products.Dtos;
using ECommerce.Infrastructure.Products.Models;
using ECommerce.Products.Features.CreatingProduct;
using ECommerce.Products.Features.GettingAllProductsByPage;

namespace ECommerce.Products.Features;
public class ProductMappings : Profile
{
    public ProductMappings()
    {
        _ = CreateMap<CreateProductRequestDto, CreateProduct>();
        _ = CreateMap<CreateProduct, Product>();
        IMappingExpression<Product, CreateProductResult> mappingExpression = CreateMap<Product, CreateProductResult>();

        _ = CreateMap<GetProductsByPageResult, GetProductsByPageResponseDto>();
        _ = CreateMap<GetProductsByPageRequestDto, GetProductsByPage>();

        _ = CreateMap<Product, ProductDto>()
            .ConstructUsing(x =>
                new ProductDto(x.Id, x.Name, x.Barcode, x.Description, x.CategoryId, x.IsBreakable, x.Price,
                    x.ProfitMargin, x.NetPrice));

        _ = CreateMap<PageList<Product>, PageList<ProductDto>>();
    }
}
