namespace ECommerce.Categories.Api.Features;

using AutoMapper;
using BuildingBlocks.Core.Pagination;
using CreatingCategory;
using ECommerce.Categories.Api.Features.GettingAllCategoriesByPage;
using ECommerce.Infrastructure.Categories.Dtos;
using ECommerce.Infrastructure.Categories.Models;
using GettingAllCategoriesByPage;

public class CategoryMappings: Profile
{
    public CategoryMappings()
    {
        CreateMap<CreateCategoryRequestDto, CreateCategory>();
        CreateMap<CreateCategory, Category>();
        CreateMap<Category, CreateCategoryResult>();

        CreateMap<GetCategoriesByPageResult, GetCategoriesByPageResponseDto>();
        CreateMap<GetCategoriesByPageRequestDto, GetCategoriesByPage>();

        CreateMap<Category, CategoryDto>()
            .ConstructUsing(x =>
                new CategoryDto(x.Id, x.Name));

        CreateMap<PageList<Category>, PageList<CategoryDto>>();
    }
}
