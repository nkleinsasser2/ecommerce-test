
using BuildingBlocks.Exception;

namespace ECommerce.Infrastructure.Categories.Exceptions;
public class CategoryAlreadyExistException : ConflictException
{
    public CategoryAlreadyExistException(int? code = default) : base("Category already exist!", code)
    {
    }
}
